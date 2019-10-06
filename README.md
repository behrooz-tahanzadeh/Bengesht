# Bengesht


The set of functionalities available in this plugin are:

### WebSocket / Grasshopper as a ROS node:
These components of Bengesht can handle (send/receive) communication with a server over WebSocket protocol. Using these modules, Grasshopper can be plugged into a ROS based network of parallel processes (Nodes). RosBridge is a ROS package which serves as a WebSocket server on the ROS side.

**ROS.GH:** a set of the open-source parser is developed to simplify the generation of RosBridge messages.
These components are added during ITECH Research Pavilion 2016/17 fabrication when Grasshopper was widely used for solving geometrical problems and path planning of robots in real-time.

### HTTP Server / Web interfaces for Grasshopper:
To make the communication between web applications and Grasshopper possible, I developed HTTP server components. HTTP is a bidirectional protocol for serving web contents. Parameters can be passed from web application for Grasshopper script, and the outcome can be sent back.

``` >> You should run Rhino as administrator to enable this functionality << ```

### Geometrical functionalities / Muqarnas:
The curves related components of Bengesht are initially created to simplify the process of modeling Muqarnas.
Later, the Divide On Intersects components was used by our structural engineers during ITECH research pavilion to prepare models for structural analysis.


### Solar Calculation:
This component calculates the position of the sun at a given time and place as a vector.

### Wiimote:
thanks to Benedikt Wannemacher I had the chance to implement this component for making a bidirectional communication between a Wiimote controller and Grasshopper.
