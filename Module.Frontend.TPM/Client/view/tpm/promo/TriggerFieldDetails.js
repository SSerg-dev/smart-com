Ext.define('App.view.tpm.promo.TriggerFieldDetails', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.triggerfielddetails',
    trigger1Cls: 'form-info-trigger',
    cls: 'promo-activity-details-window-field',
    labelCls: 'borderedField-label',
    width: '100%',
    labelWidth: 190,
    labelSeparator: '',
    trigger1Cls: 'form-info-trigger',
    editable: false,

    listeners: {
        afterrender: function (el) {
            el.triggerCell.addCls('form-info-trigger-cell')
        },
    },

    valueToRaw: function (value) {
        return Ext.util.Format.number(value, '0.00');
    },

    rawToValue: function (value) {
        var parsedValue = parseFloat(String(value).replace(Ext.util.Format.decimalSeparator, "."))
        return isNaN(parsedValue) ? null : parsedValue;
    },

    onTrigger1Click: function (el) {
        var me = this;
        var promoController = App.app.getController('tpm.promo.Promo');
        var record = promoController.getRecord(Ext.ComponentQuery.query('promoeditorcustom'));

        var window = Ext.create('App.view.core.base.BaseModalWindow', {
            title: 'Details',
            width: '90%',
            minWidth: '90%',
            height: '90%',
            minHeight: '90%',
            items: [{
                xtype: 'promoactivitydetailsinfo'
            }],
            buttons: [{
                text: l10n.ns('core', 'buttons').value('cancel'),
                itemId: 'close'
            }]
        });

        me.dataIndexes.forEach(function (field) {
            var columns = window.down('directorygrid').columns;
            columns.forEach(function (column) {
                if (field === column.dataIndex) {
                    column.hidden = false;
                }
            });
        });

        window.show();

        var promoProductStore = window.down('directorygrid').getStore();
        promoProductStore.setFixedFilter('PromoIdFilter', {
            operator: 'and',
            rules: [{
                property: 'PromoId',
                operation: 'Equals',
                value: record.data.Id
            }]
        });
        promoProductStore.load();
    }
});