﻿Ext.define('App.view.tpm.mechanic.DeletedMechanic', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedmechanic',
    title: l10n.ns('core', 'compositePanelTitles').value('deletedPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydeleteddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
		editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.mechanic.DeletedMechanic',
            storeId: 'deletedmechanicstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.mechanic.DeletedMechanic',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'DeletedDate',
                direction: 'DESC'
            }]
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1,
                minWidth: 100
            },
            items: [
            { 
                text: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate'),
				dataIndex: 'DeletedDate',
				xtype: 'datecolumn',
				renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
			},
             			{
				text: l10n.ns('tpm', 'Mechanic').value('Name'),
				dataIndex: 'Name'
			}, 			{
				text: l10n.ns('tpm', 'Mechanic').value('SystemName'),
				dataIndex: 'SystemName'
                }, {
                                text: l10n.ns('tpm', 'Mechanic').value('PromoType.Name'),
                    dataIndex: 'PromoTypeName',
                    filter: {
                        type: 'search',
                        selectorWidget: 'promotypes',
                        valueField: 'Name',
                        store: {
                            type: 'directorystore',
                            model: 'App.model.tpm.promotypes.PromoTypes',
                            extendedFilter: {
                                xclass: 'App.ExtFilterContext',
                                supportedModels: [{
                                    xclass: 'App.ExtSelectionFilterModel',
                                    model: 'App.model.tpm.promotypes.PromoTypes',
                                    modelId: 'efselectionmodel'
                                }, {
                                    xclass: 'App.ExtTextFilterModel',
                                    modelId: 'eftextmodel'
                                }]
                            }
                        }
                    }
                }
            ]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.mechanic.DeletedMechanic',
        items: [
        { 
			xtype: 'singlelinedisplayfield',
			name: 'DeletedDate',
			renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
			fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')		
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Mechanic').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('tpm', 'Mechanic').value('SystemName'),
        }]
    }]
});