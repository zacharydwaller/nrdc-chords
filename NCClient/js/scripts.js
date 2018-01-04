var svcUrl = "http://localhost:3485/DataCenter/";

var selectedNetwork = "";
var selectedDeployment = 0;
var selectedStream = 0;

var hiClasses = "list-group-item hierarchy-item";

$(document).ready(function ()
{
    $(".hierarchy-item").remove();

    // Attach netbuttonClick function to network buttons 
    $(".net-button").click(netbuttonClick);
});

function netbuttonClick()
{
    selectedNetwork = $(this).attr("value");

    $(".net-button").removeClass("active");
    $(this).addClass("active");

    $(".hierarchy-item").remove();

    // Get sites from NRDC here
    createSiteButton("Sheep X");
}

function siteButtonClick()
{
    $(".site-button").removeClass("active");
    $(this).addClass("active");

    $(".deployment-button").remove();
    $(".stream-button").remove();

    createDeploymentButton("Air Temperature");
}

function deploymentButtonClick()
{
    selectedDeployment = 0;

    $(".deployment-button").removeClass("active");
    $(this).addClass("active");

    $(".stream-button").remove();

    createStreamButton("Average (01:00 Interval)");
}

function streamButtonClick()
{
    selectedStream = 0;

    $(".stream-button").removeClass("active");
    $(this).addClass("active");
}

function fadeInButton(button)
{
    $(button).hide();
    $(button).fadeIn();
}

function createButton(text)
{
    var item = document.createElement("a");
    $(item).addClass(hiClasses);
    item.innerHTML = text;
    return item;
}

function createSiteButton(text)
{
    var item = createButton(text);
    $(item).addClass("site-button");
    $(item).click(siteButtonClick);
    $("#site-list").append(item);
    fadeInButton(item);
}

function createDeploymentButton(text)
{
    var item = createButton(text);
    $(item).addClass("deployment-button");
    $(item).click(deploymentButtonClick);
    $("#deployment-list").append(item);
    fadeInButton(item);
}

function createStreamButton(text)
{
    var item = createButton(text);
    $(item).addClass("stream-button");
    $(item).click(streamButtonClick)
    $("#stream-list").append(item);
    fadeInButton(item);
}