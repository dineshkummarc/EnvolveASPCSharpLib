using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace EnvolveAPI
{
    public class EnvolveAPIEncoder
    {
        private const String API_VERSION = "0.2";
        protected int mySiteID;
        public int SiteID
        {
            get
            {
                return mySiteID;
            }
        }
        protected String secretKey;
        public EnvolveAPIEncoder(String apiKey)
        {
            //parse the API key
            String[] pieces = apiKey.Split('-');
            if (pieces.Length == 2)
            {
                try
                {
                   this.mySiteID = int.Parse(pieces[0]);
                }
                catch (Exception e)
                {
                    throw new Exception("Invalid Envolve API Key");
                }
                this.secretKey = pieces[1];
            }
            else
            {
                throw new Exception("Invalid Envolve API Key");
            }
        }
		 
        public String getLoginCommand(String firstName, String lastName, String picture, bool isAdmin)
	    {
		    if(firstName == null)
		    {
		    	throw new Exception("You must provide at least a first name. If you are providing a username, use it for the first name and set the last name to null");
	        }
		
		    String commandString = "v=" + API_VERSION + ",c=login,fn=" + this.base64Encode(firstName);
		    if(lastName != null)
		    {
			    commandString += ",ln=" + this.base64Encode(lastName);
		    }
		    if(picture != null)
		    {
			    commandString += ",pic=" + this.base64Encode(picture);
		    }
		    if(isAdmin)
		    {
			    commandString += ",admin=t";
		    }
		    return this.signCommand(commandString);
	    }

	    public String getLogoutCommand()
	    {
		    return this.signCommand("c=logout");
	    }
	
	    private String signCommand(String command)
	    {
            //convert to a unix "epoch" time format.
            String c = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000) + ";" + command;

            String hash = this.calculateHMAC_SHA1Hash(c).ToLower();

		    return hash + ";" + c;
	    }
	
	    private string calculateHMAC_SHA1Hash(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(this.secretKey);
            HMACSHA1 cryptoService = new HMACSHA1(keyBytes);
            byte[] rawHash = cryptoService.ComputeHash(bytes);
            string hash = ByteToString(rawHash);

            return hash;
        }

        private static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        private string base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = System.Text.Encoding.UTF8.GetBytes(data);    
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch(Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }
    }
}