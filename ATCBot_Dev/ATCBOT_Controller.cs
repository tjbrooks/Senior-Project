using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

// Add these two statements to all SimConnect clients 
using LockheedMartin.Prepar3D.SimConnect;
using System.Runtime.InteropServices;


namespace ATCBot_Dev
{
    /*
     * 
     * !alt.  = We are currently at XXX ft.
       !flight = a combination of variables  "XXX mph at XXX ft. And XXX miles from our next waypoint XXX"
       !hdg = Currently flying a heading of XXX degrees
       !metar ICAO = Returns LIVE metar data for the requested ICAO
       !metar ICAO = LIVE metar for ICAO is XXXXX xXXXXXX xXXXXX XXXx your captain may or may not be using live weather.
       !plane = We are flying the XXXX by XXXX. (Livery/model by Manufacturer)  ex. AirAmbulance New Zealand Lear35 by Flight1(edited)
       !route (returns route data loaded into sim).  ICAO XXX XXX XXX XXX ICAO
       !speed = We are currently flying at XXX mph (XXX kph)(edited)
       !temp = The current outside temperature is XXX degrees
       THIS IS THE SECOND MOST ASKED QUESTION WE GET!  !time
       !time = Displays current IN SIM time that the host has set. This allows other pilots to sync their clocks for the best possible weather profile match
       !time = Current in-sim time is XXX XXXX XXXX
       !wind = Current wind data is XXXmph / XXX degrees
       !wx = Weather at our current location is X winds from X degrees. The outside temperature is X and there is (precipitation) with visibility at X miles
       I'm sure verbiage will be further massaged once we go live but if you can get it to deliver those variables into the chat, we've done our job and the rest is CAKE
 */
    /// <summary>
    /// ATCBOT_Controller is used to interface with Prepared3d. An API allows access to Prepared3d and this controller is used 
    /// to send/receive properly formatted data request based on request commands from Twitch
    /// </summary>
    class ATCBOT_Controller
    {
        private static ATCBOT_Controller instance = new ATCBOT_Controller();
        /// <summary>
        /// Instance is used to gain access to the ATCBOT_Controller object being used.
        /// </summary>
        public static ATCBOT_Controller Instance
        {
            get
            {
                return instance;
            }
        }
        /// <summary>
        /// ATCBOT_Controller constructor sends a message indicating that the contoller object has been intiated.
        /// </summary>
        ATCBOT_Controller()
        {
            ATCBOT_View.Instance.displayText("Testing message from controller");
        }

        // User-defined win32 event 
        const int WM_USER_SIMCONNECT = 0x0402;
        /// <summary>
        /// Simconnect is the main source of communication used to access Prepared3d.
        /// </summary>
        public SimConnect simconnect = null;
        /// <summary>
        /// DIFINITIONS are the structure names that simconnect will fill with requested data.
        /// </summary>
        enum DEFINITIONS
        {
            Struct_alt, Struct_flight, Struct_heading, Struct_speed, Struct_temp, Struct_time, Struct_route, Struct_plane, Struct_wind, Struct_wx, Struct1,
        }
        /// <summary>
        /// DATA_REQUESTS is an enumeration of the formatted data request that will be sent to simconnect.
        /// </summary>
        enum DATA_REQUESTS
        {
            REQUEST_ALT, REQUEST_FLIGHT, REQUEST_HEADING, REQUEST_SPEED, REQUEST_TEMP, REQUEST_TIME, REQUEST_ROUTE, REQUEST_PLANE, REQUEST_WIND, REQUEST_WX, REQUEST_1,
        };

        /// <summary>
        /// Struct_alt is used to hold the Altitude data of the flight, the attributes of which are required
        /// for proper communication with simconnect.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct_alt
        {
            // this is how you declare a fixed size string 
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public double altitude;
        };
        /// <summary>
        /// Struct_flight is used to store mulitple variables of flight data including altitude, speed, distance, and waypoint.
        /// Attributes of structure are required for particular simconnect data. 
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct_flight
        {
            //!flight = a combination of variables  "XXX mph at XXX ft. And XXX miles from our next waypoint XXX"
            // this is how you declare a fixed size string 
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public double altitude;
            public double speed;
            public double distance;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String waypoint;
        };
        /// <summary>
        /// Struct_heading is used to store the degree of direction from North. Attributes of structure are required for particular simconnect data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct_heading
        {
            // this is how you declare a fixed size string 
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public double heading;
        };
        /// <summary>
        /// Struct_speed  is used to store the speed in the simulator. Attributes of structure are required for particular simconnect data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct_speed
        {
            // this is how you declare a fixed size string 
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public double speed;
        };
        /// <summary>
        /// Struct_temp is used to store the temprature in the simulator. Attributes of structure are required for particular simconnect data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct_temp
        {
            // this is how you declare a fixed size string 
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public double temperature;
        };
        /// <summary>
        /// Struct_time is used to store the current time in the simulator. Attributes of structure are required for particular simconnect data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct_time
        {
            // this is how you declare a fixed size string 
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public double time;
        };
        /// <summary>
        /// Struct_route is used to store information pertaining to the current route set in the simulator. Attributes of structure are required for particular simconnect data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct_route
        {
            // this is how you declare a fixed size string 
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public double ete;
            public double eta;
            public double distance;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String waypoint_previous;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String waypoint_next;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String waypoint_arroach;
        };
        /// <summary>
        /// Struct_plane is used to store information about the current plane being used in the simulatore. Attributes of structure are required for particular simconnect data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct_plane
        {
            // this is how you declare a fixed size string 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String title;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String Airline;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String Type;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String Model;
        };
        /// <summary>
        /// Struct_wind is used to sture data about the current wind conditions in the simulatore. Attributes of structure are required for particular simconnect data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct_wind
        {
            public double velocity;
            public double direction;
        };

        //!wx = Weather at our current location is X winds from X degrees. The outside temperature is X and there is (precipitation) with visibility at X miles
        /// <summary>
        /// Struct_wx is used to store data about the current wheather conditions temprature, precipitation, visability, etc. Attributes of structure are required for particular simconnect data.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct_wx
        {
            public double velocity;
            public double direction;
            public double temperature;
            public int precip_state;
            public int precip_rate;
            public double visibility;

        };


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct1
        {
            // this is how you declare a fixed size string 
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String title;
            public double latitude;
            public double longitude;
            public double altitude;
        };

        /// <summary>
        /// initDataRequest is used to setup simconnect. Definitions sent informing simconnect what data is being requested and the structure to fill the data into.
        /// Event handlers are set to handle events triggered by simconnect.
        /// </summary>
        public void initDataRequests()
        {

            // Altitude
            try
            {
                // listen to connect and quit msgs 
                simconnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(simconnect_OnRecvOpen);
                simconnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(simconnect_OnRecvQuit);

                // listen to exceptions 
                simconnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(simconnect_OnRecvException);

                //
                // ALT: define a data structure 
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_alt, "Plane Altitude", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.RegisterDataDefineStruct<Struct_alt>(DEFINITIONS.Struct_alt);

                //
                // FLIGHT: define a data structure 
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_flight, "Plane Altitude", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_flight, "AIRSPEED INDICATED", "Knots", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_flight, "GPS TARGET DISTANCE", "Meters", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_flight, "NAV IDENT", "ICAO code", SIMCONNECT_DATATYPE.STRING32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.RegisterDataDefineStruct<Struct_flight>(DEFINITIONS.Struct_flight);

                //
                // HEADING: define a data structure 
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_heading, "HEADING INDICATOR", "Radians", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.RegisterDataDefineStruct<Struct_heading>(DEFINITIONS.Struct_heading);

                //
                // SPEED: define a data structure 
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_speed, "AIRSPEED INDICATED", "Knots", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.RegisterDataDefineStruct<Struct_speed>(DEFINITIONS.Struct_speed);

                //
                // TEMP: define a data structure 
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_temp, "AMBIENT TEMPERATURE", "Celsius", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.RegisterDataDefineStruct<Struct_temp>(DEFINITIONS.Struct_temp);

                //
                // TIME: define a data structure 
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_time, "LOCAL TIME", "Seconds", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.RegisterDataDefineStruct<Struct_time>(DEFINITIONS.Struct_time);

                //
                // ROUTE: define a data structure 
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_route, "GPS TARGET DISTANCE", "Meters", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_route, "GPS WP NEXT ID", null, SIMCONNECT_DATATYPE.STRING32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_route, "GPS WP PREV ID", null, SIMCONNECT_DATATYPE.STRING32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_route, "GPS ETA", "seconds", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_route, "GPS ETE", "seconds", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_route, "GPS APPROACH Airport Id", null, SIMCONNECT_DATATYPE.STRING32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.RegisterDataDefineStruct<Struct_route>(DEFINITIONS.Struct_route);

                //
                // PLANE: define a data structure 
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_plane, "title", null, SIMCONNECT_DATATYPE.STRING256, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_plane, "ATC AIRLINE", null, SIMCONNECT_DATATYPE.STRING256, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_plane, "ATC TYPE", null, SIMCONNECT_DATATYPE.STRING256, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_plane, "ATC MODEL", null, SIMCONNECT_DATATYPE.STRING256, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.RegisterDataDefineStruct<Struct_plane>(DEFINITIONS.Struct_plane);


                //
                // WIND: define a data structure 
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_wind, "AMBIENT WIND VELOCITY", "Feet per second", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_wind, "AMBIENT WIND DIRECTION", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.RegisterDataDefineStruct<Struct_wind>(DEFINITIONS.Struct_wind);

                //
                // WX: define a data structure 

                simconnect.AddToDataDefinition(DEFINITIONS.Struct_wx, "AMBIENT WIND VELOCITY", "Feet per second", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_wx, "AMBIENT WIND DIRECTION", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_wx, "AMBIENT TEMPERATURE", "Celsius", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_wx, "AMBIENT PRECIP STATE", "Mask", SIMCONNECT_DATATYPE.INT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_wx, "AMBIENT PRECIP RATE", "Enum", SIMCONNECT_DATATYPE.INT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct_wx, "AMBIENT VISIBILITY", "Meters", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.RegisterDataDefineStruct<Struct_wx>(DEFINITIONS.Struct_wx);


                // Struct1: define a data structure 
                simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "title", null, SIMCONNECT_DATATYPE.STRING256, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Altitude", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simconnect.RegisterDataDefineStruct<Struct1>(DEFINITIONS.Struct1);

                // catch a simobject data request 
                simconnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(simconnect_OnRecvSimobjectDataBytype);
            }
            catch (COMException ex)
            {
                String myText = "Error on simconnect: " + ex.Message;
                //ATCBOT_View.Instance.displayText
                //ATCBOT_View.getInstance().displayText(myText);
                ATCBOT_View.Instance.displayText(myText);
                //ATCBOT_View.getInstance.
                //ATCBOT_View.displayText(myText);

            }
        }

        /// <summary>
        /// simconnect_OnRecvOpen indicates to the client that a successful connection has been made with the simulator.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        void simconnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            ATCBOT_View.Instance.displayText("Connected to Prepar3D");
        }

        /// <summary>
        /// simconnect_OnRecvQuit indicates to the client that the simulator has closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        void simconnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            ATCBOT_View.Instance.displayText("Prepar3D has exited");
            closeConnection();
        }
        /// <summary>
        /// simconnect_OnRecvException indicates to the client that an error has occured with the resulting error displayed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        void simconnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            ATCBOT_View.Instance.displayText("Exception received: " + data.dwException);
        }
        /// <summary>
        /// Get_Alt is used to recieve the altitude in the current simulation.
        /// </summary>
        public void Get_alt()
        {
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_ALT, DEFINITIONS.Struct_alt, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            //simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_1, DEFINITIONS.Struct1, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            ATCBOT_View.Instance.displayText("Request sent...");
        }


        /// <summary>
        /// simconnect_OnRecvSimobjectDataByType is used to handle events from simconnect, the events are based on the commands it is sent <see cref="getFlightData(string)"/>.
        /// The requested data is pulled from the respective structure and in some cases formatted.
        /// A string describing the data and the data itself is then sent to the client window and to Twitch. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        void simconnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            string message = "";

            switch ((DATA_REQUESTS)data.dwRequestID)
            {
                case DATA_REQUESTS.REQUEST_ALT:
                    Struct_alt s_alt = (Struct_alt)data.dwData[0];
                    message = "We are currently at " + Math.Truncate(s_alt.altitude * 1) / 1 + " ft.";
                    break;
                case DATA_REQUESTS.REQUEST_FLIGHT:
                    Struct_flight s_flight = (Struct_flight)data.dwData[0];
                    message = "Currently flying at " + Math.Truncate(s_flight.altitude * 10) / 10 + " feet with a speed of " + Math.Truncate(s_flight.speed * 10) / 10 + " knots. ";
                    if (s_flight.distance > 0)
                    {
                        message = message + "Currently " + Math.Truncate(s_flight.distance * 10) / 10 + " from our next waypoint of " + s_flight.waypoint;
                    }
                    break;
                case DATA_REQUESTS.REQUEST_HEADING:
                    Struct_heading s_heading = (Struct_heading)data.dwData[0];
                    double degrees = Math.Truncate(s_heading.heading * 180 / 3.1415 * 1) / 1;
                    message = "Currently flying a heading of " + degrees + " degrees";
                    break;
                case DATA_REQUESTS.REQUEST_SPEED:
                    Struct_speed s_speed = (Struct_speed)data.dwData[0];
                    //We are currently flying at XXX mph(XXX kph)(edited)
                    double mph = Math.Truncate(s_speed.speed * 0.868976 * 1) / 1;
                    message = "Currently flying at " + Math.Truncate(s_speed.speed * 1) / 1 + " knots or " + mph + " miles per hour.";
                    break;
                case DATA_REQUESTS.REQUEST_TEMP:
                    Struct_temp s_temp = (Struct_temp)data.dwData[0];
                    //We are currently flying at XXX mph(XXX kph)(edited)
                    //!temp = The current outside temperature is XXX degrees
                    double farenheit = Math.Truncate((s_temp.temperature * 9 / 5 + 32) * 1) / 1;
                    message = "The current outside temperature is " + Math.Truncate(s_temp.temperature * 1) / 1 + " C, or " + farenheit + " F.";
                    break;
                case DATA_REQUESTS.REQUEST_TIME:
                    Struct_time s_time = (Struct_time)data.dwData[0];
                    //We are currently flying at XXX mph(XXX kph)(edited)
                    //!time = Current in-sim time is XXX XXXX XXXX
                    int hours = (int)(s_time.time / 3600);
                    int minutes = (int)((s_time.time % 3600) / 60);
                    int seconds = (int)((s_time.time % 3600) % 60);
                    message = "Current in-sim time is " + hours + " hours  " + minutes + " minutes " + seconds + " seconds";
                    break;
                case DATA_REQUESTS.REQUEST_ROUTE:
                    Struct_route s_route = (Struct_route)data.dwData[0];
                    message = "Currently flying from " + s_route.waypoint_previous + " to " + s_route.waypoint_next + " with ete " + s_route.ete + " ending at "
                       + s_route.waypoint_arroach + " with eta of " + s_route.eta + ".";

                    break;
                case DATA_REQUESTS.REQUEST_PLANE:
                    Struct_plane s_plane = (Struct_plane)data.dwData[0];
                    message = "Currently flying with livery " + s_plane.title + ". ATC Info: " + s_plane.Airline + " " + s_plane.Type + " " + s_plane.Model + ".";

                    break;
                case DATA_REQUESTS.REQUEST_WIND:
                    Struct_wind s_wind = (Struct_wind)data.dwData[0];
                    //double wind_degrees = Math.Truncate(s_wind.direction * 180 / 3.1415 * 1) / 1;
                    double velocity_mph = Math.Truncate(s_wind.velocity * .681818 * 10) / 10;
                    message = "Current wind velocity is " + velocity_mph + " mph. Wind direction is " + s_wind.direction + " degrees.";

                    break;
                case DATA_REQUESTS.REQUEST_WX:
                    Struct_wx s_wx = (Struct_wx)data.dwData[0];
                    //double wind_degrees = Math.Truncate(s_wind.direction * 180 / 3.1415 * 1) / 1;
                    //!wx = Weather at our current location is X winds from X degrees.The outside temperature is X and there is (precipitation)with visibility at X miles
                    double velocity_wx_mph = Math.Truncate(s_wx.velocity * .681818 * 1) / 1;
                    double farenheit_wx = Math.Truncate((s_wx.temperature * 9 / 5 + 32) * 1) / 1;
                    message = velocity_wx_mph + "mph at " + s_wx.direction + " degrees.";
                    message = message + "The outside temperature is " + Math.Truncate(s_wx.temperature * 1) / 1 + " C,  " + farenheit_wx + " F. ";
                    //message = message + "Visibility is " + s_wx.visibility + " meters. ";

                    switch (s_wx.precip_state)
                    {
                     case 2:
                        message = message + "There is no precipitation.";
                        break;
                     case 4:
                        switch(s_wx.precip_rate)
                        {
                        case 0:
                           message = message + "There is light rain.";
                           break;
                        case 1:
                           message = message + "There is light rain.";
                           break;
                        case 2:
                           message = message + "There is moderate rain.";
                           break;
                        case 3:
                           message = message + "There is heavy rain.";
                           break;
                        case 4:
                           message = message + "There is extremely heavy rain.";
                           break;
                        }
                        break;
                     case 8:
                        switch (s_wx.precip_rate)
                        {
                           case 0:
                              message = message + "There is light snow.";
                              break;
                           case 1:
                              message = message + "There is light snow.";
                              break;
                           case 2:
                              message = message + "There is moderate snow.";
                              break;
                           case 3:
                              message = message + "There is heavy snow.";
                              break;
                           case 4:
                              message = message + "There is extremely heavy snow.";
                              break;
                        }
                        break; 
                    }
               break;
           case DATA_REQUESTS.REQUEST_1:
               Struct1 s2 = (Struct1)data.dwData[0];

               ATCBOT_View.Instance.displayText("title: " + s2.title);
               ATCBOT_View.Instance.displayText("Lat:   " + s2.latitude);
               ATCBOT_View.Instance.displayText("Lon:   " + s2.longitude);
               ATCBOT_View.Instance.displayText("Alt:   " + s2.altitude);
               break;
           default:
               ATCBOT_View.Instance.displayText("Unknown request ID: " + data.dwRequestID);

               break;
       }
       ATCBOT_View.Instance.displayText(message);
       Twitch_Controller.Instance.sendChatMessage(message);
   }
   /// <summary>
   /// openConnection initiates a connection with simconnect and a request for data <see cref="initDataRequests"/>. Errors result in a connection not being made and message being sent to the client. 
   /// </summary>
   public void openConnection()
   {
       if (simconnect == null)
       {
           try
           {
               // the constructor is similar to SimConnect_Open in the native API 
               simconnect = new SimConnect("Managed Data Request", ATCBOT_View.Instance.Handle, WM_USER_SIMCONNECT, null, 0);

               //setButtons(false, true, true);

               initDataRequests();


           }
           catch (COMException ex)
           {
               ATCBOT_View.Instance.displayText("Unable to connect to Prepar3D:\n\n" + ex.Message);
           }
       }
       else
       {
           ATCBOT_View.Instance.displayText("Error - try again");
           closeConnection();

           //setButtons(true, false, false);
       }

   }
   /// <summary>
   /// closeConnection disconnects the system from simconnect.
   /// </summary>
   public void closeConnection()
   {
       if (simconnect != null)
       {
           // Dispose serves the same purpose as SimConnect_Close() 
           simconnect.Dispose();
           simconnect = null;
           ATCBOT_View.Instance.displayText("Connection closed");
       }
   }

   /// <summary>
   /// getFlightData recieves commands from <see cref="Twitch_Listener"/> data for the specified command sent through the simconnect object. 
   /// Simconnect will fill a specified structure for the specific data being requested and trigger an event <see cref="initDataRequests"/>
   /// </summary>
   /// <param name="command">Command is the in the form of a bang command recieved from Twitch.</param>
   public void getFlightData(string command)
   {
       switch (command)
       {
         case "!alt":
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_ALT, DEFINITIONS.Struct_alt, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            break;
         case "!flight":
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_FLIGHT, DEFINITIONS.Struct_flight, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            break;
         case "!hdg":
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_HEADING, DEFINITIONS.Struct_heading, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            break;
         case "!metar":
            /* Not supported for this build */
            break;
         case "!plane":
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_PLANE, DEFINITIONS.Struct_plane, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            break;
         case "!route":
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_ROUTE, DEFINITIONS.Struct_route, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            break;
         case "!speed":
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_SPEED, DEFINITIONS.Struct_speed, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            break;
         case "!temp":
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_TEMP, DEFINITIONS.Struct_temp, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            break;
         case "!time":
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_TIME, DEFINITIONS.Struct_time, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            break;
         case "!wind":
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_WIND, DEFINITIONS.Struct_wind, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            break;
         case "!wx":
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_WX, DEFINITIONS.Struct_wx, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            break;
         default:
            break;
         }
     }



   }
}
