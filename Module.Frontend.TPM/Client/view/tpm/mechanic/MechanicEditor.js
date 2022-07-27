Ext.define('App.view.tpm.mechanic.MechanicEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.mechaniceditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [
            {
                xtype: 'textfield',
                name: 'Name',
                fieldLabel: l10n.ns('tpm', 'Mechanic').value('Name'),
            },
            {
                xtype: 'textfield',
                name: 'SystemName',
                fieldLabel: l10n.ns('tpm', 'Mechanic').value('SystemName'),
            },
            {
                xtype: 'searchfield',
                fieldLabel: l10n.ns('tpm', 'Mechanic').value('PromoType.Name'),
                name: 'PromoTypesId',
                selectorWidget: 'promotypes',
                valueField: 'Id',
                displayField: 'Name',
                store: {
                    type: 'directorystore',
                    model: 'App.model.tpm.promotypes.PromoTypes',
                    extendedFilter: {
                        xclass: 'App.ExtFilterContext',
                        supportedModels: [{
                            xclass: 'App.ExtSelectionFilterModel',
                            model: 'App.model.tpm.promotypes.PromoTypes',
                            modelId: 'efselectionmodel'
                        }]
                    }
                },
                mapping: [{
                    from: 'Name',
                    to: 'PromoTypes.Name'
                }]
            }
        ]
    }
});