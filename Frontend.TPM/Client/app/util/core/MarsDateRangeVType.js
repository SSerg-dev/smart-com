Ext.apply(Ext.form.field.VTypes, {
    marsDateRangeText: l10n.ns('core', 'customValidators').value('StartDateFinishDateOrder'),

    marsDateRange: function (value, field) {
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

        return startDate.toDate().getTime() < finishDate.toDate().getTime();
    }
});