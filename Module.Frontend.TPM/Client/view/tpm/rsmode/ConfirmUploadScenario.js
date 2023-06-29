Ext.define('App.view.tpm.rsmode.ConfirmUploadScenario', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.confirmUploadScenario',
    title: l10n.ns('tpm', 'ConfirmUploadScenario').value('ConfirmUploadScenario'),

    width: 350,
    minWidth: 350,
    minHeight: 200,
    height: 200,
    items: [{
        xtype: 'container',
        cls: 'custom-promo-panel-container',
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        flex: 1,
        items: [
            {
                xtype: 'container',
                padding: 10,
                layout: {
                    type: 'hbox',
                    align: 'middle'
                },
                flex: 1,
                items: [
                    {
                        xtype: 'searchfield',
                        name: 'SavedScenarioId',
                        fieldLabel: l10n.ns('tpm', 'ConfirmUploadScenario').value('SavedScenario'),
                        selectorWidget: 'savedScenario',
                        valueField: 'Id',
                        displayField: 'ScenarioName',
                        store: {
                            type: 'directorystore',
                            model: 'App.model.tpm.savedScenario.SavedScenario',
                            extendedFilter: {
                                xclass: 'App.ExtFilterContext',
                                supportedModels: [{
                                    xclass: 'App.ExtSelectionFilterModel',
                                    model: 'App.model.tpm.savedScenario.SavedScenario',
                                    modelId: 'efselectionmodel'
                                }]
                            }
                        },
                    }
                ]
            },
        ],
    }],

    buttons: [
        {
            text: l10n.ns('core', 'buttons').value('cancel'),
            itemId: 'close'
        },
        {
            text: l10n.ns('core', 'buttons').value('confirm'),
            itemId: 'confirm'
        }
    ]
})
