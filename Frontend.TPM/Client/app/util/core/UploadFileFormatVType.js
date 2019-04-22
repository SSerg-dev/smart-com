Ext.apply(Ext.form.field.VTypes, {
    filePassText: l10n.ns('core', 'customValidators').value('UploadFileFormat'),

    isValidFileFormat: function (format, value) {
        var regex = new RegExp('.*\\.(' + format + ')$', 'i');
        return regex.test(value);
    },

    filePass: function (value, field) {
        var result = true;
        if (field.allowFormat && typeof (field.allowFormat) == 'string') {
            result = this.isValidFileFormat(field.allowFormat, value);
        } else if (field.allowFormat && Ext.isArray(field.allowFormat)) {
            result = false;
            for (var i = 0; i < field.allowFormat.length; i++) {
                if (this.isValidFileFormat(field.allowFormat[i], value)) {
                    result = true;
                }
            }
        }
        return result;
    }
});