Ext.define('App.localization.core.LocalizationContext', {

    constructor: function (data) {
        this.localizationData = data;
        this.namespaceNames = [];
    },

    ns: function () {
        var namespaceName = '.' + Ext.Array.toArray(arguments).join('.');
        this.namespaceNames.push(namespaceName);
        return this;
    },

    value: function (propertyName) {
        var localizationData = this.localizationData;

        if (!Ext.isString(propertyName)) {
            return '';
        }

        if (Ext.isEmpty(this.namespaceNames)) {
            this.namespaceNames.push('');
        }

        var namespaces = this.namespaceNames.reduce(function (result, namespaceName) {
            if (localizationData.hasOwnProperty(namespaceName)) {
                return result.concat(localizationData[namespaceName]);
            }
            console.error(Ext.String.format('Namespace: \'{0}\' not found', namespaceName));
            return result;
        }, []);

        if (Ext.isEmpty(namespaces)) {
            return '';
        }

        var namespace = Ext.Array.findBy(namespaces, function (namespace) {
            return namespace.hasOwnProperty(propertyName);
        });

        if (namespace) {
            return namespace[propertyName];
        } else {
            console.error(Ext.String.format('Property \'{0}\' not found in namespaces: \'{1}\'', propertyName, this.namespaceNames.join('\', \'')));
        }

        return '';
    }

});