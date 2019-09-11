Ext.define('Ext.ux.data.CustomValidators', {
    alternateClassName: 'CustomValidators',
    singleton: true,

    getValidator: function (name) {
        return this.validators && this.validators.getByKey(name);
    },

    define: function (validators) {
        var validValidators = Ext.Array.from(validators)
         .filter(function (item) {
             var isValidMessage = item.message && Ext.isString(item.message),
                 isValidValidator = item.isValid && Ext.isFunction(item.isValid),
                 isValidName = item.name && Ext.isString(item.name);
             return isValidMessage && isValidValidator && isValidName;
         }, this);

        this.validators = new Ext.util.MixedCollection({
            getKey: function (el) {
                return el.name;
            }
        });
        this.validators.addAll(validValidators);
    }

});