var today = new Date();
var day, month;

if (today.getDate() <= 9)
{
    day = 0 + String(today.getDate());
}
else
{
    day = today.getDate();
}

if (today.getMonth()<=9)
{
    month = 0 + String((today.getMonth() + 1));
}
else
{
    month = today.getMonth() + 1
}
document.getElementById("user-birth-date").setAttribute("max", today.getFullYear() + '-' + month + '-' + day);
$("#form_date").data("DateTimePicker").date(null);
