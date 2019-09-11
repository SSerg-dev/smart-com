Ext.define('Ext.ux.form.Extended', {
    extend: 'Ext.form.Basic',

    setValues: function (values) {
        var me = this,
            v, vLen, val, field;

        function setVal(fieldId, val) {
            var field = me.findField(fieldId);
            if (field) {
                if (field.isSearchField) {
                    me.setSearchFieldValue(field, val);
                } else {
                    field.setValue(val);
                }
                if (me.trackResetOnLoad) {
                    field.resetOriginalValue();
                }
            }
        }

        // Suspend here because setting the value on a field could trigger
        // a layout, for example if an error gets set, or it's a display field
        Ext.suspendLayouts();
        if (Ext.isArray(values)) {
            // array of objects
            vLen = values.length;

            for (v = 0; v < vLen; v++) {
                val = values[v];

                setVal(val.id, val.value);
            }
        } else {
            // object hash
            Ext.iterate(values, setVal);
        }
        Ext.resumeLayouts(true);
        return this;
    },

    setSearchFieldValue: function (field, val) {
        var rec = this.getRecord(),
            mapping = Ext.Array.from(field.mapping);

        var dfMap = Ext.Array.findBy(mapping, function (map) {
            return map.from === field.displayField;
        });

        if (dfMap) {
            field.setValue({
                value: val,
                display: rec.get(dfMap.to)
            });
        } else {
            console.error('Mapping for dispalyField does not set at field', field.getName());
        }
    }

});