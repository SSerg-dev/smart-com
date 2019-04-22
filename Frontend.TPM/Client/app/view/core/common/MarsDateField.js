// ------------------------------------------------------------------------
// <copyright file="MarsDate.js" company="Смарт-Ком">
//     Copyright statement. All right reserved
// </copyright>
// Проект Transportation Workflow System
// Версия 1.0
// Автор: Сергей Токарев (EMail: sergey.tokarev@smart-com.su)
// ------------------------------------------------------------------------

Ext.define('App.view.core.common.MarsDateField', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.marsdatefield',

    triggerCls: 'x-form-date-trigger',
    editable: true,
    weekRequired: false,

    onTriggerClick: function () {
        var picker = Ext.widget('marsdatepicker', {
            pickerField: this,
            weekRequired: this.weekRequired,

            listeners: {
                scope: this,
                select: this.onSelect
            }

        });

        var val = this.getValue();

        if (!val) {
            var now = new App.MarsDate();
            var week = undefined;
            if (this.weekRequired) {
                week = now.getWeek();
            }
            val = new App.MarsDate(now.getYear(), now.getPeriod(), week);
        }

        picker.setValue(val);
        picker.show();
    },

    validator: function (value) {
        var result = true;
        if (!Ext.isEmpty(value)) {
            var marsDate = App.MarsDate.parse(value);
            if (marsDate !== null) {
                if (this.weekRequired && marsDate.format === App.MarsDate.MD_FORMAT_P) {
                    return l10n.ns('core', 'marsCalendar', 'errors').value('weekRequired');
                }
            } else {
                result = l10n.ns('core', 'marsCalendar', 'errors').value('invalidFormat');
            }
        }
        return result;
    },

    valueToRaw: function (value) {
        return value ? value.toString() : value;
    },

    rawToValue: function (rawValue) {
        return this.parseMarsDate(rawValue) || rawValue || null;
    },

    parseMarsDate: function (value) {
        return App.MarsDate.parse(value);
    },

    onSelect: function (m, d) {
        this.setValue(d);
        this.gridRecordUpdate();
        this.fireEvent('select', this, d);
    },

    gridRecordUpdate: function () {
        var me = this;
        var value = me.value;
        var grid = me.up('grid');
        if (grid) {
            var plugin = grid.editingPlugin;
            if (plugin) {
                var context = plugin.context;
                if (context) {
                    var record = context.record;
                    if (record) {
                        if (!record.isEqual(value, context.value)) {
                            record.set(context.field, value);
                        }
                    }
                    context.value = value;
                }
            }
        }
    },

    initComponent: function () {
        this.addEvents('select');
        this.callParent(arguments);
    }

});