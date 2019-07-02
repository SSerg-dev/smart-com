// Валидация диапазона "дата начала" - "дата окончания"
// Для корректной работы у поля "дата начала" должно быть свойство finishDateField со значением = name поля "дата окончания", у поля "дата окончания", соответственно startDateField
Ext.apply(Ext.form.field.VTypes, {
    dateRangeText: l10n.ns('core', 'customValidators').value('StartDateFinishDateOrder'),

    dateRange: function (value, field) {
        var form = field.up('form'),
            startDateField, finishDateField,
            startDate, finishDate;

        if (field.startDateField) {
            startDateField = form.down('[name=' + field.startDateField + ']');
            startDate = startDateField ? startDateField.getValue() : null;
            finishDate = field.getValue();
        } else if (field.finishDateField) {
            startDate = field.getValue();
            finishDateField = form.down('[name=' + field.finishDateField + ']');
            finishDate = finishDateField ? finishDateField.getValue() : null;
        } else {
            return true;
        }

        if (!startDate || !finishDate) {
            return true;
        }
        var valid = startDate.getTime() < finishDate.getTime();
        // если диапазон валиден снимаем подсветку и со второго поля тоже
        if (valid) {
            if (startDateField) {
                startDateField.clearInvalid()
            } else {
                finishDateField.clearInvalid();
            }         
        }
        return valid
    }
});