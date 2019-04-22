Ext.define('App.view.core.constraint.UserRoleMain', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.userrolemain',
    title: l10n.ns('core', 'compositePanelTitles').value('UserRoleMainTitle'),

    dockedItems: [{
        xtype: 'readonlywithoutfilterdirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.core.userrole.ConstraintUserRole',
            storeId: 'userrolemaintore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.userrole.ConstraintUserRole',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
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
                text: l10n.ns('core', 'UserRoleMain').value('Login'),
                dataIndex: 'Login',
                filter: {
                    type: 'search',
                    plugins: ['usersearchfield'],
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.core.user.User',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.core.user.User',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('core', 'UserRoleMain').value('DisplayName'),
                dataIndex: 'RoleDisplayName',
                filter: {
                    type: 'search',
                    selectorWidget: 'role',
                    valueField: 'DisplayName',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.core.role.Role',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.core.role.Role',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.userrole.ConstraintUserRole',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'Login',
            fieldLabel: l10n.ns('core', 'UserRoleMain').value('Login')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'RoleDisplayName',
            fieldLabel: l10n.ns('core', 'UserRoleMain').value('DisplayName')
        }]
    }]

});