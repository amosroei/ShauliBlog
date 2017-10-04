
    var appid = "2e9a3b3f3b7a98e0442e3a85875a2481";
            $(document).ready(function () {
        $("button").click(function () {
            $.get("http://api.openweathermap.org/data/2.5/weather?q=TelAviv&units=metric&APPID=" + appid + "&units=imperial", function (response) {
                //response
                console.log(response);
                $("#name").text(response.name);
                $("#temp").text(response.main.temp);
                $("#humidity").text(response.main.humidity);
            });
        });
    });

      