var dateConvertTimeZone = function (value, record) {
    if (!record.phantom && !record.editing && value && value != '') {
        return changeTimeZone(value, this.timeZone);
    }
    else {
        return value;
    }
}

var changeTimeZone = function (value, timeZone, direction) {
    if (value && value instanceof Date) {
        var dir = direction || 1;
        var currentTimeZone = value.getTimezoneOffset() / -60;
        var differenceHour = timeZone - currentTimeZone;
        var date = new Date(value);

        date.setHours(date.getHours() + dir * differenceHour);
        return date;
    }
    else {
        return value;
    }
}