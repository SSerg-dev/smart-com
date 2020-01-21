Ext.define('App.view.tpm.promo.PromoChangeStatusWindow', {
    extend: 'App.view.core.common.SelectorWindow',
    alias: 'widget.promochangestatuswindow',
    title: l10n.ns('tpm', 'text').value('selectpromostatuswindow'),

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promostatus.PromoStatus',
            storeId: 'promostatusstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promostatus.PromoStatus',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            fixedFilters: {
                'SystemName': {
                    property: 'SystemName',
                    operation: 'NotEqual',
                    value: 'Deleted'
                }   
            }
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1,
                minWidth: 100
            },
            items: [{
                text: l10n.ns('tpm', 'PromoStatus').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('tpm', 'PromoStatus').value('SystemName'),
                dataIndex: 'SystemName'
            }, {
                text: l10n.ns('tpm', 'PromoStatus').value('Color'),
                dataIndex: 'Color',
                renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
                    //var descr = record.get('Color');
                    return Ext.String.format('<div style="background-color:{0};width:50px;height:10px;display:inline-block;margin:0 5px 0 5px;border:solid;border-color:gray;border-width:1px;"></div>', record.get('Color'));
                }
            }]
        }
    }]
})