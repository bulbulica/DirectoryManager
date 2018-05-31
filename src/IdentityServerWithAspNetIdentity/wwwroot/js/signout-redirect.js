function redirectWithTimeOut(url, timeout)
{
    window.setTimeout(function () {
        location.href = url;
    }, timeout);
}

$(document).ready(function () {
    if ($("#PostLogoutUrl") && $("#PostLogoutUrl").length > 0)
    {
        url = $("#PostLogoutUrl")[0].href;
        if (url)
        {
            redirectWithTimeOut(url, 1000);
        }
        
    }
    else
    {
        redirectWithTimeOut("../../", 1000);
    }
});