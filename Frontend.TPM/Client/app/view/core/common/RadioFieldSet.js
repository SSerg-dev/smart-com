Ext.define('App.view.core.common.RadioFieldSet', {
    extend: 'Ext.form.FieldSet',
    alias: 'widget.radiofieldset',
    checkboxToggle: true,
    padding: '10 10 0 10',

    groupName: 'group',
    checked: false,

    initItems: function () {
        this.items = [{
            xtype: 'fieldcontainer',
            margin: 0,

            layout: {
                type: 'vbox',
                align: 'stretch'
            },

            defaults: {
                ui: 'detail-form-field',
                labelClsExtra: 'singleline-lable',
                labelAlign: 'left',
                labelWidth: 170,
                labelSeparator: '',
                labelPad: 0
            },

            items: this.items
        }];

        this.callParent();
    },

    createCheckboxCmp: function () {
        var me = this,
            suffix = '-checkbox';

        me.checkboxCmp = Ext.widget({
            xtype: 'checkbox',
            hideEmptyLabel: true,
            name: me.checkboxName || me.id + suffix,
            cls: me.baseCls + '-header' + suffix,
            id: me.id + '-legendChk',
            checked: me.checked,
            msgTarget: 'none',
            name: me.checkBoxDataIndex,
            listeners: {
                change: me.onCheckChange,
                scope: me
            }
        });

        return me.checkboxCmp;
    },

    onCheckChange: function () {
        var value = this.checkboxCmp.getValue(),
            fieldsets = this.up().query('fieldset[groupName=' + this.groupName + '][id!=' + this.getId() + ']');

        fieldsets.forEach(function (item) {
            item.checkboxCmp.setValue(!value);
            if (item.container) {
                item.container.setDisabled(value);
                item.container.query('field').forEach(function (field) {
                    field.reset();
                }, this);
            }
        }, this);
    },

    onRender: function () {
        this.callParent();
        this.container = this.down('fieldcontainer');
        this.container.setDisabled(!this.checkboxCmp.getValue());
    }
});