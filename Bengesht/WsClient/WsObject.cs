using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;




namespace Bengesht.WsClient
{
	class WsObject
	{
		private WebSocket webSocket;
		public int status;
		public string message;
		private string initMessage;
		public event EventHandler changed;
		



		public WsObject init(string address, string initMessage)
		{
			this.webSocket = new WebSocket(address);
			this.initMessage = initMessage;
			this.webSocket.WaitTime = new TimeSpan(0, 0, 2);
			this.connect();
			return this;
		}




		protected virtual void onChanged()
		{
			EventHandler handler = changed;

			if (handler != null)
				handler(this, EventArgs.Empty);
		}




		private WsObject connect()
		{
			this.webSocket.OnOpen += this.onOpen;
			this.webSocket.OnMessage += this.onMessage;
			this.webSocket.OnError += this.onError;
			this.webSocket.OnClose += this.onClose;

			this.webSocket.ConnectAsync();

			return this;
		}




		public WsObject disconnect()
		{
			this.webSocket.OnOpen -= this.onOpen;
			this.webSocket.OnMessage -= this.onMessage;
			this.webSocket.OnError -= this.onError;
			this.webSocket.OnClose -= this.onClose;

			this.webSocket.Close();

			return this;
		}




		private void onOpen(object sender, EventArgs e)
		{
			send(initMessage);
			status = WsObjectStatus.OPEN;
			onChanged();
		}




		private void onError(object sender, ErrorEventArgs e)
		{
			status = WsObjectStatus.CLOSE;
			webSocket = null;
			onChanged();
		}




		private void onMessage(object sender, MessageEventArgs e)
		{
			status = WsObjectStatus.MESSAGE;

			if (!e.IsPing)
				message = e.Data;

			onChanged();
		}




		private void onClose(object sender, CloseEventArgs e)
		{
			status = WsObjectStatus.CLOSE;
			webSocket = null;
			onChanged();
		}




		public void send(string msg)
		{
			if(webSocket != null && webSocket.IsConnected)
				webSocket.Send(msg);
		}
	}




	class WsObjectStatus
	{
		public static int ERROR = 0;
		public static int OPEN = 1;
		public static int MESSAGE = 2;
		public static int CLOSE = 3;

		public static string GetStatusName(int status)
		{
			switch(status)
			{
			case 0:
				return "Error";
			case 1:
				return "Open";
			case 2:
				return "Message";
			case 3:
				return "Close";
			}

			return "UNKOWN";
		}
	}
}
