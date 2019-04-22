Ext.define('Ext.ux.form.Panel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.extendedpanel',

    createForm: function () {
        var cfg = {},
            props = this.basicFormConfigs,
            len = props.length,
            i = 0,
            prop;

        for (; i < len; ++i) {
            prop = props[i];
            cfg[prop] = this[prop];
        }

        return new Ext.ux.form.Extended(this, cfg);
    }

});