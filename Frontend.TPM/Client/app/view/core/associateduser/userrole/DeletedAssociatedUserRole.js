﻿Ext.define('App.view.core.associateduser.userrole.DeletedAssociatedUserRole', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedassociateduserrole',
    title: l10n.ns('core', 'compositePanelTitles').value('deletedPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.core.associateduser.userrole.DeletedAssociatedUserRole',
            storeId: 'historicaluserrolestore',
            autoLoad: true,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.associateduser.userrole.DeletedAssociatedUserRole',
                    modelId: 'efselectionmodel'
                }]
            },
            sorters: [{
                property: '_EditDate',
                direction: 'DESC'
            }],
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
                text: l10n.ns('core', 'DeletedAssociatedUserRole').value('_User'),
                dataIndex: '_User',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('core', 'DeletedAssociatedUserRole').value('_Role'),
                dataIndex: '_Role',
                filter: {
                    type: 'string',
                    operator: 'eq'
                }
            }, {
                text: l10n.ns('core', 'DeletedAssociatedUserRole').value('_EditDate'),
                dataIndex: '_EditDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('core', 'DeletedAssociatedUserRole').value('_Operation'),
                dataIndex: '_Operation',
                renderer: App.RenderHelper.getLocalizedRenderer('core.DeletedAssociatedUserRole', 'OperationType'),
                filter: {
                    type: 'combo',
                    valueField: 'id',
                    store: {
                        type: 'operationtypestore'
                    },
                    operator: 'eq'
                }
            }]
        }
    }, {
        xtype: 'detailform',
        itemId: 'detailform',
        model: 'App.model.core.associateduser.userrole.DeletedAssociatedUserRole',
        items: [{
            name: '_User',
            fieldLabel: l10n.ns('core', 'DeletedAssociatedUserRole').value('_User')
        }, {
            name: '_Role',
            fieldLabel: l10n.ns('core', 'DeletedAssociatedUserRole').value('_Role')
        }, {
            name: '_EditDate',
            fieldLabel: l10n.ns('core', 'DeletedAssociatedUserRole').value('_EditDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
        }, {
            name: '_Operation',
            fieldLabel: l10n.ns('core', 'DeletedAssociatedUserRole').value('_Operation'),
            renderer: App.RenderHelper.getLocalizedRenderer('core.DeletedAssociatedUserRole', 'OperationType')
        }, {
            name: 'RoleDisplayName',
            fieldLabel: l10n.ns('core', 'DeletedAssociatedUserRole').value('RoleDisplayName')
        }, {
            name: 'IsDefault',
            fieldLabel: l10n.ns('core', 'DeletedAssociatedUserRole').value('IsDefault')
        }]
    }]
});