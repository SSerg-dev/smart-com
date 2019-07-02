Ext.apply(Ext.form.field.VTypes, {
    extendEmailText: l10n.ns('core', 'customValidators').value('EmailValidationError'),

    extendEmailMask: /[\w.\-@'"!#$%&'*+/=?^_`{|}~]/i,
    // стандартная валидация Extjs не пропускает адрес с длиной домена более 6 символов (.software не пройдёт)
    extendEmail: function (value) {
        var email = /^(")?(?:[^\."\s])(?:(?:[\.])?(?:[\w\-!#$%&'*+/=?^_`{|}~]))*\1@(\w[\-\w]*\.){1,5}([A-Za-z]){2,8}$/;
        return email.test(value);
    }
});