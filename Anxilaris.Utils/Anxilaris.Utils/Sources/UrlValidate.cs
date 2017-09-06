//-----------------------------------------------------------------------
// <copyright file="UrlValidate.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Ramon Castillo Guevara</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    public class UrlValidate
    {
        /// <summary>
        /// Validate a url response Method "GET"
        /// </summary>
        /// <param name="url">url to evaluate</param>
        /// <param name="timeout">timeot in miliseconds To wait for the request</param>
        /// <returns>Object with a Boolean property with validation and a type string with message</returns>
        public static URLRequest UrlIsValid(string url, int timeout)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = timeout; //set the timeout to 5 seconds to keep the user from waiting too long for the page to load
                //request.Method = "HEAD"; //Get only the header information -- no need to download any content
                // Set the credentials to the current user account
                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                request.Method = "GET";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                int statusCode = (int)response.StatusCode;
                if (statusCode >= 100 && statusCode < 400) //Good requests
                {
                    return new URLRequest { IsValid = true, Message = "OK, Good requests" };
                }
                else if (statusCode >= 500 && statusCode <= 510) //Server Errors
                {
                    return new URLRequest { IsValid = false, Message = String.Format("The remote server has thrown an internal error. Url is not valid: {0}", url) };
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError) //400 errors
                {
                    return new URLRequest { IsValid = false, Message = "400 error" };
                }
                else
                {
                    return new URLRequest { IsValid = false, Message = ex.Message };
                }
            }
            catch (Exception ex)
            {
                //log.Error(String.Format("Could not test url {0}.", url), ex);
                return new URLRequest { IsValid = false, Message = String.Format("Could not test url {0}.", url) };
            }
            return new URLRequest { IsValid = false, Message = "Some Error unknown" };
        }
        /// <summary>
        /// Validate a url response Method "POST"
        /// </summary>
        /// <param name="url">url to evaluate</param>
        /// <param name="var">Arrangement of variables to be attached to the request POST, Must be the same size as the array of values </param>
        /// <param name="values">Arrangement of values to be attached to the request POST, Must be the same size as the array of variables </param>
        /// <param name="timeout">timeot in miliseconds To wait for the request</param>
        /// <returns>Object with a Boolean property with validation and a type string with message </returns>
        public static URLRequest UrlIsValid(string url, List<RouteValues> Variables, int timeout)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = timeout; //set the timeout to 5 seconds to keep the user from waiting too long for the page to load
                request.Method = "POST"; //Get only the header information -- no need to download any content
                request.ContentType = "application/x-www-form-urlencoded";
                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                if (Variables != null)
                {
                    var postData = string.Empty;
                    for (int i = 0; i < Variables.Count; i++)
                    {
                        if(i> 0)
                        {
                            postData += "&";
                        }
                        postData += Variables[i].Variable + "=" + Variables[i].Value;
                    }
                    var data = Encoding.ASCII.GetBytes(postData);
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }               

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                int statusCode = (int)response.StatusCode;
                if (statusCode >= 100 && statusCode < 400) //Good requests
                {
                    return new URLRequest { IsValid = true, Message = "OK, Good requests" };
                }
                else if (statusCode >= 500 && statusCode <= 510) //Server Errors
                {
                    return new URLRequest { IsValid = false, Message = String.Format("The remote server has thrown an internal error. Url is not valid: {0}", url) };
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError) //400 errors
                {
                    return new URLRequest { IsValid = false, Message = "400 error" };
                }
                else
                {
                    return new URLRequest { IsValid = false, Message = ex.Message };
                }
            }
            catch (Exception ex)
            {
                //log.Error(String.Format("Could not test url {0}.", url), ex);
                return new URLRequest { IsValid = false, Message = String.Format("Could not test url {0}.", url) };
            }
            return new URLRequest { IsValid = false, Message = "Some Error unknown" };
        }
    }
    
}
public class URLRequest
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
}
public class RouteValues
{
    public string Variable { get; set; }
    public string Value { get; set; }
}
