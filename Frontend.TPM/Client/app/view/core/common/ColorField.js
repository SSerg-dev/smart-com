Ext.define('App.view.core.common.ColorField', {
    extend: 'Ext.form.field.Picker',
    alias: 'widget.colorfield',
    editable: false,
    matchFieldWidth: false,
    pickerAlign: 'tr-br?',
    regex: /^[0-9a-f]{3,6}$/i,
    defaultColor: '#ffffff',
    emptyTextStr: l10n.ns('core').value('colorEmptyText'),

    createPicker: function () {
        var me = this;

        return new Ext.widget('colormenu', {
            pickerField: me,
            focusOnShow: true,
            listeners: {
                scope: me,
                select: me.onSelect
            },
            keyNavConfig: {
                esc: function () {
                    me.collapse();
                }
            }
        });
    },

    onSelect: function (colorpicker) {
        this.setValue(colorpicker.getValue());
    },

    clearValue: function () {
        this.setValue(null);
    },

    setValue: function (value) {
        this.emptyText = value ? '' : this.emptyTextStr;
        this.callParent(arguments);
        this.setFieldStyle({
            'background-color': this.value ? '#' + this.value : this.defaultColor,
            'background-image': 'none'
        });

        if (this.inputEl) {
            this.inputEl.dom.value = null;
        }
    },

    getValue: function () {
        return this.value;
    },

    getRawValue: function () {
        return this.value || this.callParent();
    },

    getSubTplData: function () {
        var data = this.callParent();
        data.value = null;
        return data;
    },

    applyEmptyText: function () {
        var me = this,
            emptyText = me.emptyText,
            isEmpty;

        if (me.rendered) {
            isEmpty = me.getRawValue().length < 1 && !me.hasFocus;

            if (Ext.supports.Placeholder) {
                me.inputEl.dom.placeholder = emptyText;
            } else if (isEmpty) {
                me.setRawValue(emptyText);
                me.valueContainsPlaceholder = true;
            }

            if (isEmpty) {
                me.inputEl.addCls(me.emptyCls);
            }

            me.autoSize();
        }
    }
});