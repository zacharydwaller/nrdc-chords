var serviceUrl = "http://localhost:3485/";
//var serviceUrl = "http://nrdcstudentweb.rd.unr.edu/team_16/Services/NCInterface/";
var chordsUrl = "http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com/";

var selectedNetwork;
var selectedSite;
var selectedSystem;
var selectedDeployment;
var selectedStreams;

var hiClasses = "list-group-item hierarchy-item";

var loader = "<img id=\"loading\" src=\"img/spinner.gif\" style=\"width:100px\" />";
var noSessions = "<p id='no-sessions' class='text-muted'>No sessions found.</p>";
var noConnection = "<h2 id='no-connections' class='text-danger'>Connection to NRDC-CHORDS Web Service unable to be made.</h2>"

var systemsHeader;
var deploymentsHeader;
var streamsHeader;
var selectedStreamsHeader;

var startCalendar;
var endCalendar;

var sessionKey;
var intervalFunc;

var defaultInterval = 60; // in seconds
var serviceUrl = "http://localhost:3485/";
var selectedNetwork = "NevCAN";
var uri = serviceUrl + "DataCenter/" + selectedNetwork + "/sites?";



function initMap(data) {
    var uri = serviceUrl + "DataCenter/" + selectedNetwork + "/sites?";
    //var siteName = notExpandHierarchy(uri, returnLocation);
    // var siteName = returnLocation();
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 6,
        center: new google.maps.LatLng(37.85, -115.6003),
        mapTypeId: google.maps.MapTypeId.terrain,
    });
    var networks = ["nevCan", "WalkerBasinHydro", "SolarNexus"];
    var infowindow = new google.maps.InfoWindow(); 
    var marker, i;
    for (networksIndex = 0; networksIndex < networks.length; networksIndex++)
    {
        uri = serviceUrl + "DataCenter/" + networks[networksIndex] + "/sites?";
        expandHierarchy(uri, function (data) { 
            for (i = 0; i < data.length; i++)
            { 
                marker = new google.maps.Marker
                    ({
                        position: new google.maps.LatLng(data[i].Latitude, data[i].Longitude),
                        map: map 
                    });
                google.maps.event.addListener(marker, 'click', (function (marker, i)
                    {
                        return function () {
                            infowindow.setContent(data[i]["Alias"]);
                            infowindow.open(map, marker);
                        }
                    })(marker, i));
            }
        });    
    }
}

$(document).ready(function () {
    // initMap();
    initialize();

});

function initialize() {
    selectedNetwork = "NevCAN";
    selectedSite = 0;
    selectedSystem = 0;
    selectedDeployment = 0;
    selectedStreams = new Set();

    populateSessionList();

    $("#netTab").click();

    $(".hierarchy-item").remove();

    // Attach netbuttonClick function to network buttons 
    $(".net-button").click(netbuttonClick);

    getHeaders();
    hideHeaders();

    attemptConnection();

    // Retrieve NevCAN sites
    expandHierarchy(serviceUrl + "DataCenter/" + selectedNetwork + "/sites?", expandSites);
 
    // Initialize Visualize tab
    $("#VisOptions").show();
    $("#VisResult").hide();

    startCalendar = $("#startdate");
    endCalendar = $("#enddate");

    startCalendar.datepicker();
    endCalendar.datepicker();

    getData(uri, initMap);
}

function sessionButtonClick() {
    sessionKey = $(this).attr("value");
    interval = defaultInterval * 1000;

    $("#VisContent").click();
    $("#VisOptions").hide();
    $("#VisResult").show();

    refreshSession();
    intervalFunc = setInterval(refreshSession, interval);

    $("#sessionKey").append("Resuming Session: " + sessionKey);

    openChordsSession(sessionKey);
}




function netbuttonClick() {
    selectedNetwork = $(this).attr("value");

    $(".net-button").removeClass("active");
    $(this).addClass("active");

    $(".hierarchy-item").remove();
    $(".site-button").remove();

    expandHierarchy(serviceUrl + "DataCenter/" + selectedNetwork + "/sites?", expandSites);

    $("#streamTab").click();
}

function siteButtonClick() {
    selectedSite = $(this).attr("value");

    $(".site-button").removeClass("active");
    $(this).addClass("active");

    $(".system-button").remove();
    $(".deployment-button").remove();
    $(".stream-button").remove();

    $("#system-list").append(loader);
    expandHierarchy(serviceUrl + "DataCenter/" + selectedNetwork + "/systems?siteID=" + selectedSite, expandSystems);
}

function systemButtonClick() {
    selectedSystem = $(this).attr("value");

    $(".system-button").removeClass("active");
    $(this).addClass("active");

    $(".deployment-button").remove();
    $(".stream-button").remove();

    $("#deployment-list").append(loader);
    expandHierarchy(serviceUrl + "DataCenter/" + selectedNetwork + "/deployments?systemID=" + selectedSystem, expandDeployments);
}

function deploymentButtonClick() {
    selectedDeployment = $(this).attr("value");

    $(".deployment-button").removeClass("active");
    $(this).addClass("active");

    $(".stream-button").remove();

    $("#stream-list").append(loader);
    expandHierarchy(serviceUrl + "DataCenter/" + selectedNetwork + "/streams?deploymentID=" + selectedDeployment, expandStreams);
}

function streamButtonClick() {
    var id = $(this).attr("value");

    if (selectedStreams.has(id)) {
        selectedStreams.delete(id);
        $(this).removeClass("active");
    }
    else {
        selectedStreams.add(id);
        $(this).addClass("active");
    }

    updateSelectedStreams();
}

function selstreamButtonClick() {
    var id = $(this).attr("value");

    selectedStreams.delete(id);

    $(".stream-button").filter(function () { return $(this).attr("value") == id }).removeClass("active");
    updateSelectedStreams();
}

function attemptConnection() {
    uri = serviceUrl + "NCInterface/";

    console.log("Attempting to connect to NRDC-CHORDS Web service");

    $.ajax({
        type: "GET",
        dataType: "json",
        url: uri,

        xhrFields: {
            withCredentials: false
        },

        success: function (result) {
            console.log("Connected succeeded");
        },

        error: function () {
            console.log("Connection failed");
            $("#no-connection").append(noConnection);
        }
    });
}

function visualizeButtonClick() {
    uri = serviceUrl + "Session/newSession?"
        + "netAlias=" + selectedNetwork;

    // Add list of streams
    for (let id of selectedStreams) {
        uri += "&streamIDs=" + id;
    }

    // Add name and description
    uri += "&name=" + $("#sessionName").val();
    uri += "&description=" + $("#sessionDescription").val();

    // Add start and end date
    var start = startCalendar.datepicker("getDate");
    var end = endCalendar.datepicker("getDate");

    if (start != null) {
        var date = new Date(start);

        uri += "&startTime=" + date.toISOString();
    }

    if (end != null) {
        var date = new Date(end);

        uri += "&endTime=" + date.toISOString();
    }

    console.log(uri);

    $("#VisOptions").hide();

    $("#VisResult").show();
    $("#VisResult").append(loader);
    $("#sessionError").hide();
    $("#sessionInstructions").show();

    $.ajax({
        type: "GET",
        dataType: "json",
        url: uri,

        xhrFields: {
            withCredentials: false
        },

        success: function (result) {
            $("#loading").remove();

            if (result.Success) {
                $("#sessionKey").show();
                $("#sessionError").hide();

                sessionKey = result.Data;
                $("#sessionKey").append("Your Session Key: " + sessionKey);
                openChordsSession(sessionKey);

                refreshSession();

                interval = defaultInterval * 1000;
                intervalFunc = setInterval(refreshSession, interval);

                console.log(interval);

                // Refresh session list
                populateSessionList();
            }
            else {
                $("#sessionKey").empty();
                $("#sessionInstructions").hide();
                $("#sessionError").show();
                $("#sessionError").append(result.Message);
                console.error(result.Message);

                $("#VisOptions").show();
                //$("#VisResult").hide();
            }
        },

        error: function () {
            $("#loading").remove();

            console.error("Call to " + uri + " unable to be completed.");

            $("#sessionError").append("Call to " + uri + " unable to be completed.");
        }
    });
}

function refreshSession() {
    uri = serviceUrl + "Session/refreshSession?"
        + "key=" + sessionKey;

    console.log("Refreshed");

    $.ajax({
        type: "GET",
        dataType: "json",
        url: uri,

        xhrFields: {
            withCredentials: false
        },

        success: function (result) {
            if (!result.success) {
                $("#sessionKey").append("<br>" + result.Message);
                clearInterval(intervalFunc);
            }
        },

        error: function () {
            console.error("Call to " + uri + " unable to be completed.");
            console.error(result.Message);

            $("#VisResult").append("Error: Call to " + uri + " unable to be completed.");
            $("#VisResult").append(result.Message);
        }
    });
}

function openChordsSession(key) {
    var uri = serviceUrl + "Session/GetSession?key=" + key;

    $.ajax({
        type: "GET",
        dataType: "json",
        url: uri,

        xhrFields: {
            withCredentials: false
        },

        success: function (result) {
            if (result.Success) {
                window.open(chordsUrl + "instruments/" + result.Data[0].InstrumentID, "_blank");
            }
            else {
                //$("#sessionKey").append(result.Message);
                console.error(result.Message);

                $("#VisOptions").show();
                $("#VisResult").hide();
            }
        },

        error: function () {
            $("#loading").remove();

            console.error("Call to " + uri + " unable to be completed.");

            $("#VisResult").append("Call to " + uri + " unable to be completed.");
        }
    });
}

function updateSelectedStreams() {
    if (selectedStreams.size > 0) {
        selectedStreamsHeader.show();
    }
    else {
        selectedStreamsHeader.hide();
    }

    expandSelectedStreams();
}

function fadeInButton(button) {
    $(button).hide();
    $(button).fadeIn();
}

function createButton(buttonClass, listName, callback) {
    var item = document.createElement("a");

    $(item).addClass(hiClasses);
    $(item).addClass(buttonClass);
    $(item).click(callback);
    $(listName).append(item);

    fadeInButton(item);

    return item;
}

function populateSessionList() {
    var uri = serviceUrl + "Session/GetSessionList";

    //$("#session-list").append(loader);

    $.ajax({
        type: "GET",
        dataType: "json",
        url: uri,

        xhrFields: {
            withCredentials: false
        },

        success: function (result) {
            $("#session-list").empty();

            //console.log(result);
            if (result.Success == true) {
                if (result.Data.length == 0) {
                    $("#session-list").append(noSessions);
                }

                for (var i = 0; i < result.Data.length; i++) {
                    var session = result.Data[i];
                    var button = createButton("session-button", "#session-list", sessionButtonClick);

                    // Add session name to button
                    button.innerHTML = "<h3 class=\"list-group-item-heading\">" + session.Name

                    // Add session description to button if description is set
                    if (session.Description != null && session.Description != "") {
                        button.innerHTML += "<p class=\"list-group-item-text\">" + session.Description + "</p>"
                    }

                    // Add rest of info to button
                    button.innerHTML
                        += "<p class=\"list-group-item-heading\">" + session.NetworkAlias + "</p>"
                        + "<p class=\"list-group-item-text\">" + "CHORDS Instrument: " + session.InstrumentID + "</p>"
                        + "<p class=\"list-group-item-text\">" + "Last Refresh: " + session.LastRefresh + "</p>";

                    $(button).attr("value", session.SessionKey);
                }
            }
            else {
                console.error(result.Message);
            }
        },

        error: function () {
            $("#loading").remove();

            console.error("Call to " + uri + " unable to be completed.");
        }
    });
}

function returnLocation(data) {
    testVar = data
  // console.log(data);
    return String(testVar);
}


function getData(uri, callback) {
    
    $.ajax({
        type: "get",
        datatype: "json",
        url: uri,

        xhrfields: {
            withcredentials: false
        },

        success: function (result) {
            $("#loading").remove();
            callback(result.Data[0]["Alias"]);
             
            
        },

        error: function () {
            $("#loading").remove();
            console.error("call to " + uri + " unable to be completed.");
        }
    });

     
    
}

function expandHierarchy(uri, callback) {
    $.ajax({
        type: "GET",
        dataType: "json",
        url: uri,

        xhrFields: {
            withCredentials: false
        },

        success: function (result) {
            $("#loading").remove();

            //console.log(result);
            if (result.Success == true) {
                callback(result.Data);
            }
            else {
                console.error(result.Message);
            }
        },

        error: function () {
            $("#loading").remove();

            console.error("Call to " + uri + " unable to be completed.");
        }
    });
}



function expandSites(data) {
    hideHeaders();
    $(".site-button").remove();

    for (var i = 0; i < data.length; i++) {
        var button = createButton("site-button", "#site-list", siteButtonClick);
        button.innerHTML = data[i].Alias;
        $(button).attr("value", data[i].ID)
    }
}

function expandSystems(data) {
    systemsHeader.show();
    deploymentsHeader.hide();
    streamsHeader.hide();

    for (var i = 0; i < data.length; i++) {
        var button = createButton("system-button", "#system-list", systemButtonClick);
        button.innerHTML = data[i].Name;
        $(button).attr("value", data[i].ID)
    }
}

function expandDeployments(data) {
    deploymentsHeader.show();
    streamsHeader.hide();

    for (var i = 0; i < data.length; i++) {
        var button = createButton("deployment-button", "#deployment-list", deploymentButtonClick);
        button.innerHTML = data[i].Name;
        $(button).attr("value", data[i].ID)
    }
}

function returnData(data) {

    var name = data[0].Property.Name;
    return name.toString();
}

function expandStreams(data) {
    streamsHeader.show();

    for (var i = 0; i < data.length; i++) {
        var button = createButton("stream-button", "#stream-list", streamButtonClick);
        button.innerHTML =
            "<h4 class=\"list-group-item-heading\">" + data[i].ID + " - " + data[i].Property.Name + "</h4>" +
            "<p class=\"list-group-item-heading\">" + data[i].DataType.Name + "</p>" +
            "<p class=\"list-group-item-text\">" + "(" + data[i].Units.Abbreviation + ")" + "</p>" +
            "<p class=\"list-group-item-text\">" + "Interval: " + data[i].MeasurementInterval + "</p>";
        $(button).attr("value", data[i].ID)

        if (selectedStreams.has(data[i].ID.toString())) {
            $(button).addClass("active");
        }
    }
}

function expandSelectedStreams() {
    $(".selstream-button").remove();

    for (let id of selectedStreams) {
        var button = createButton("selstream-button", "#selstream-list", selstreamButtonClick);
        button.innerHTML = id;
        $(button).attr("value", id)
    }
}

function getHeaders() {
    systemsHeader = $("#systems-header");
    deploymentsHeader = $("#deployments-header");
    streamsHeader = $("#streams-header");
    selectedStreamsHeader = $("#selectedstreams-header");
}

function hideHeaders() {
    systemsHeader.hide();
    deploymentsHeader.hide();
    streamsHeader.hide();
    selectedStreamsHeader.hide();
}