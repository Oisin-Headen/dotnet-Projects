// Write your Javascript code.
$(document).ready(function () {
    var days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];
    var periods = ["am", "pm"];
    var url = "https://localhost:44384/api/ABCCC/"

    $.getJSON(url + "Cineplexes", function (data) {
        $("#CineplexSelector select").html("<option value=\"-1\">--Select a Cineplex--</option>");
        for (var item in data) {
            $("#CineplexSelector select").append("<option value=\"" + data[item].cineplexId + "\">" + data[item].location + "</option>");
        }
    });

    $("#CineplexSelector select").change(function () {
        if ($("#CineplexSelector select").val() == -1) {
            $("#MovieSelector").addClass("hidden");
            $("#DaySelector").addClass("hidden");
            $("#FastBookSubmit").addClass("hidden");
        }
        else {
            $("#FastBookSubmit").addClass("hidden");
            $("#DaySelector").addClass("hidden");
            $("#MovieSelector select").html("<option value=\"-1\">--Select a Movie--</option>");
            $.getJSON(url + "MoviesForCineplex/" + $("#CineplexSelector select option:selected").val(), function (data) {
                for (var item in data) {
                    $("#MovieSelector select").append("<option value=\"" + data[item].movieId + "\">" + data[item].title + "</option>");
                }
                $("#MovieSelector").removeClass("hidden");
            });
        }
    });

    $("#MovieSelector select").change(function () {
        if ($("#MovieSelector select").val() == -1) {
            $("#DaySelector").addClass("hidden");
            $("#FastBookSubmit").addClass("hidden");
        }
        else {
            $("#FastBookSubmit").addClass("hidden");
            $("#DaySelector select").html("<option value=\"-1\">--Select a Day--</option>");
            $.getJSON(url + "Sessions/" + $("#CineplexSelector select option:selected").val() +
                "/" + $("#MovieSelector select option:selected").val(),
                function (data) {

                    var sessionInfo = new Array();
                    for (var item in data) {
                        $("#DaySelector select").append("<option value=\"" + data[item].day + "\">" +
                            days[data[item].day] + " " + data[item].hour + ":00 " + periods[data[item].period] + "</option>");
                        sessionInfo.push({ "hour": data[item].hour, "period": data[item].period });
                    }
                    $("#DaySelector").removeClass("hidden");


                    $("#DaySelector select").change(function () {
                        if ($("#DaySelector select").val() == -1) {
                            $("#FastBookSubmit").addClass("hidden");
                        }
                        else {
                            // Subtracts one from the index to get the number for the sessionInfo
                            $("#Hour").val(sessionInfo[$("#DaySelector select option:selected").index() - 1].hour)
                            $("#Period").val(sessionInfo[$("#DaySelector select option:selected").index() - 1].period)
                            $("#FastBookSubmit").removeClass("hidden");
                        }
                    });
                });
        }
    });
});
