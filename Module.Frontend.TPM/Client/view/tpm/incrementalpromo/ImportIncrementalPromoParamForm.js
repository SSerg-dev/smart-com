Ext.define('App.view.tpm.incrementalpromo.ImportIncrementalPromoParamForm', {
    extend: 'App.view.core.common.ImportParamForm',
    alias: 'widget.importforecastparamform',

    initFields: function (fieldValues) {
        this.callParent([fieldValues]);
    },

    items: [{
        xtype: 'fsearchfield',
        name: 'clientFilter',
        fieldLabel: l10n.ns('tpm', 'BaseLine').value('ClientTreeDemandCode'),
        selectorWidget: 'baseclienttreeview',
        valueField: 'BOIstring',
        displayField: 'BOIstring',
        multiSelect: true,
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.baseclienttreeview.BaseClientTreeView',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.baseclienttreeview.BaseClientTreeView',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
        }
    }]
});