Ext.define('Ext.ux.data.ExtendedModel', {
    extend: 'Ext.data.Model',
    idProperty: undefined,

    validate: function () {
        var errors = this.callParent(arguments),
            customValidators = Ext.Array.from(this.customValidators);

        customValidators.forEach(function (name) {
            var validator = Ext.ux.data.CustomValidators.getValidator(name);

            if (validator) {
                var isValid = validator.isValid(this);

                if (!isValid) {
                    var errorObjects = Ext.Array.from(validator.fields).map(function (item) {
                        return {
                            field: item,
                            message: validator.message
                        };
                    }, this);
                    errors.add(errorObjects);
                }
            }
        }, this);

        return errors;
    }

});