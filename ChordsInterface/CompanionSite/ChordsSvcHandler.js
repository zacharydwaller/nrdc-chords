/*
    ChordsSvcHandler.js
    Handles communication with the NRDC-CHORDS Web Service

    Uses jquery.soap, developed by Remy Blom (doedje)
    https://github.com/doedje/jquery.soap
*/

// Initialize soap
$(document).ready(function ()
{
    $.soap
        ({
            url: 'http://localhost:8733/Design_Time_Addresses/Chords/Service/mex/',
            namespaceQualifier: 'WebService'
        })
    
});

function getSiteList()
{
    $.soap
        ({
            method: 'GetSite',

            data:
            {
                siteID: '1'
            },

            success: function (soapResponse)
            {
                alert(soapResponse.toString())
            },

            error: function (soapResponse)
            {
                alert(soapResponse.toString())
            }
        })
}