using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;



namespace Bengesht.HttpCat
{
    class HttpHandler
    {
        private HttpListener httpListener;
        private HttpListenerContext httpListenerContext;
        private HttpListenerRequest httpListenerRequest;
        private HttpListenerResponse httpListenerResponse;
        private string prefix;
        private ThreadStart threadRef;
        private Thread thread;
        public event EventHandler changed;
        public List<string> valueList;
        public List<string> keyList;
        private bool isWaitingForResponse;
        public bool canNotStartListener;
        public string canNotStartListenerMessage;



        public HttpHandler init(string prefix)
        {
            this.prefix = prefix;
            this.valueList = new List<string>();
            this.keyList = new List<string>();
            this.isWaitingForResponse = false;

            this.httpListener = new HttpListener();
            this.httpListener.Prefixes.Add(this.prefix);
            this.canNotStartListener = false;
            this.canNotStartListenerMessage = "";

            return this;
        }//eof



        public void start()
        {
            if(!this.isRunning)
            {
                try
                {
                    this.httpListener.Start();
                    this.threadRef = new ThreadStart(this.startListenning);
                    this.thread = new Thread(this.threadRef);
                    this.thread.Start();
                }
                catch (HttpListenerException e)
                {
                    this.canNotStartListener = true;

                    if (e.ErrorCode == 5)
                        this.canNotStartListenerMessage = "Access is denied, Try running Rhino as administrator...";
                    else if (e.ErrorCode == 183)
                        this.canNotStartListenerMessage = e.Message;
                    else
                        this.canNotStartListenerMessage = "unknown error";
                    return;
                }
                catch
                {
                    this.canNotStartListener = true;
                    this.canNotStartListenerMessage = "unknown error";
                    return;
                }
            }
        }//eof




        private void startListenning()
        {
            try
            {
                while(this.isRunning)
                {
                    this.httpListenerContext = this.httpListener.GetContext();
                    this.httpListenerRequest = this.httpListenerContext.Request;

                    this.keyList.Clear();
                    this.valueList.Clear();

                    for (int i = 0; i < this.httpListenerRequest.QueryString.AllKeys.Length; ++i)
                    {
                        this.keyList.Add(this.httpListenerRequest.QueryString.GetKey(i));
                        this.valueList.Add(this.httpListenerRequest.QueryString.GetValues(i)[0]);
                    }

                    this.httpListenerResponse = this.httpListenerContext.Response;
                    this.isWaitingForResponse = true;
                    this.onChanged();

                    while(this.isWaitingForResponse){}
                }
            }
            catch{}
        }//eof



        protected virtual void onChanged()
        {
            EventHandler handler = changed;

            if (handler != null)
                handler(this, EventArgs.Empty);
        }//eof


        public void response(string responseString)
        {
            if(this.isWaitingForResponse)
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                this.httpListenerResponse.ContentLength64 = buffer.Length;
                System.IO.Stream output = this.httpListenerResponse.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                output.Close();

                this.isWaitingForResponse = false;
            }
        }



        public void stop()
        {
            if(this.isRunning)
            {
                this.httpListener.Close();
                this.httpListener.Abort();
                this.isWaitingForResponse = false;
            }
        }

        public bool isRunning
        {
            get { return this.httpListener != null && this.httpListener.IsListening; }
        }

    }//eoc
}//eons
