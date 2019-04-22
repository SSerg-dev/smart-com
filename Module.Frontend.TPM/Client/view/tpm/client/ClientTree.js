Ext.define('App.view.tpm.client.ClientTree', {
    extend: 'App.view.core.common.CombinedDirectoryTreePanel',
    alias: 'widget.clienttree',
    title: l10n.ns('tpm', 'compositePanelTitles').value('ClientTree'),
    customHeaderItems: null,

    systemHeaderItems: [],
    minHeight: 555,

    tools: [{
        xtype: 'button',
        itemId: 'dateFilter',
        text: '00.00.0000',
        cls: 'custom-date-button',
        glyph: 0xf0f6
    }],

    dockedItems: [{
        xtype: 'customtoptreetoolbar',
        dock: 'top',
        items: [{
            xtype: 'container',
            height: '100%',
            flex: 1,
            margin: '0 10px 0 10px',
            layout: {
                type: 'hbox',
                align: 'middle',
            },
            items: [{
                triggerCls: Ext.baseCSSPrefix + 'form-clear-trigger',
                xtype: 'trigger',
                itemId: 'clientsSearchTrigger',
                hideLabel: true,
                editable: true,
                cls: 'tree-search-text-def',
                width: 250,
                onTriggerClick: function () {
                    var me = this;
                    me.setRawValue('Client search');
                    me.addClass('tree-search-text-def');
                    me.fireEvent('applySearch');
                    me.triggerBlur();
                    me.blur();
                },
                listeners: {
                    afterrender: function (field) {
                        field.setRawValue('Client search');
                    },
                    focus: function (field) {
                        if (field.getRawValue() == 'Client search') {
                            field.setRawValue('');
                            field.removeCls('tree-search-text-def');
                        }
                    },
                    blur: function (field) {
                        if (field.getRawValue() == '') {
                            field.setRawValue('Client search');
                            field.addClass('tree-search-text-def');
                        }
                    },
                }
            }, {
                xtype: 'checkbox',
                labelSeparator: '',
                itemId: 'baseClientsCheckbox',
                boxLabel: 'Base Clients',
                labelAlign: 'right',
                style: 'margin-left: 10px',
                /*listeners: {
                    focus: function (checkbox) {
                        checkbox.fieldLabelTip.setDisabled(true);
                        checkbox.fieldValueTip.setDisabled(true);
                    },
                }*/
            }, {
                xtype: 'tbspacer',
                flex: 10
            },/*{
            itemId: 'moveNode',
            glyph: 0xf252,
            text: 'Move',
            tooltip: 'Move',
            hierarchyOnly: true,
            needDisable: true
        },*/{
                xtype: 'button',
                action: 'Post',
                cls: 'hierarchyButton hierarchyButtonAdd',
                itemId: 'addNode',
                glyph: 0xf415,
                tooltip: 'Add Node',
                hierarchyOnly: true,
                needDisable: true,
            }, {
                xtype: 'button',
                action: 'Delete',
                cls: 'hierarchyButton hierarchyButtonDelete',
                itemId: 'deleteNode',
                glyph: 0xf5ad,
                tooltip: 'Delete Node',
                hierarchyOnly: true,
                needDisable: true,
            }]
        }, {
            xtype: 'container',
            height: '100%',
            flex: 1,
            cls: 'hierarchydetailform-header',
            layout: {
                type: 'hbox',
                align: 'middle',
                pack: 'center'
            },
            items: [{
                xtype: 'tbspacer',
                flex: 1
            }, {
                xtype: 'button',
                cls: 'hierarchyButton hierarchyButtonList',
                itemId: 'outletList',
                text: 'Outlets List',
                tooltip: 'Outlets list',
                icon: '/'
            }]
        }]
    }],

    items: [{
        xtype: 'container',
        cls: 'combined-directory-tree-panel-items-container',
        layout: {
            type: 'hbox',
        },
        items: [{
            xtype: 'clienttreegrid',
            minWidth: 263,
            flex: 1,
            height: '100%',
        }, {
            xtype: 'splitter',
            itemId: 'splitter_1',
            cls: 'custom-tree-panel-splitter',
            collapseOnDblClick: false,
            listeners: {
                dblclick: {
                    fn: function (event, el) {
                        var cmp = Ext.ComponentQuery.query('splitter[itemId=splitter_1]')[0];
                        cmp.tracker.getPrevCmp().flex = 1;
                        cmp.tracker.getNextCmp().flex = 1;
                        cmp.ownerCt.updateLayout();
                    },
                    element: 'el'
                }
            }
        }, {
            minWidth: 263,
            flex: 1,
            height: '100%',
            xtype: 'editorform',
            cls: 'hierarchydetailform',
            layout: 'fit',
            header: null,
            columnsCount: 1,
            items: [{
                xtype: 'custompromopanel',
                autoScroll: true,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    xtype: 'fieldset',
                    title: 'Parameters',
                    layout: {
                        type: 'vbox',
                        align: 'stretch',
                    },
                    defaults: {
                        padding: '0 3 0 3',
                    },
                    items: [{
                        xtype: 'singlelinedisplayfield',
                        name: 'ObjectId',
                        width: 280,
                        fieldLabel: l10n.ns('tpm', 'ClientTree').value('ClientHierarchyCode'),
                    }, {
                        xtype: 'singlelinedisplayfield',
                        name: 'Type',
                        width: 280,
                        fieldLabel: l10n.ns('tpm', 'ClientTree').value('Type'),
                    }, {
                        xtype: 'singlelinedisplayfield',
                        name: 'Name',
                        width: 280,
                        fieldLabel: l10n.ns('tpm', 'ClientTree').value('Name'),
                    }, {
                        xtype: 'singlelinedisplayfield',
                        name: 'OutletCount',
                        width: 280,
                        fieldLabel: l10n.ns('tpm', 'ClientTree').value('OutletCount')
                    }, {
                        xtype: 'singlelinedisplayfield',
                        name: 'ExecutionCode',
                        width: 280,
                        fieldLabel: l10n.ns('tpm', 'ClientTree').value('ExecutionCode')
                    }, {
                        xtype: 'singlelinedisplayfield',
                        name: 'DemandCode',
                        width: 280,
                        fieldLabel: l10n.ns('tpm', 'ClientTree').value('DemandCode')
                    }, {
                        xtype: 'singlelinedisplayfield',
                        name: 'Share',
                        width: 280,
                        fieldLabel: l10n.ns('tpm', 'ClientTree').value('Share')
                    }, {
                        xtype: 'singlelinedisplayfield',
                        name: 'IsBaseClient',
                        renderer: App.RenderHelper.getBooleanRenderer('Yes', 'No'),
                        width: 280,
                        fieldLabel: l10n.ns('tpm', 'ClientTree').value('IsBaseClient')
                    }, {
                        xtype: 'singlelinedisplayfield',
                        name: 'RetailTypeName',
                        width: 280,
                        fieldLabel: l10n.ns('tpm', 'RetailType').value('Name')
                    }, {
                        xtype: 'singlelinedisplayfield',
                        name: 'PostPromoEffectW1',
                        width: 280,
                        fieldLabel: l10n.ns('tpm', 'ClientTree').value('PostPromoEffectW1')
                    }, {
                        xtype: 'singlelinedisplayfield',
                        name: 'PostPromoEffectW2',
                        width: 280,
                        fieldLabel: l10n.ns('tpm', 'ClientTree').value('PostPromoEffectW2')
                    }]
                }, {
                    xtype: 'fieldset',
                    cls: 'dispatch-settings-fieldset',
                    title: l10n.ns('tpm', 'Dispatch').value('DispatchTitle'),
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 3 0 3',
                        minHeight: 52
                    },
                    items: [{
                        xtype: 'fieldcontainer',
                        cls: 'dispatchsettingsfieldcontainer',
                        itemId: 'fieldcontainerStart',
                        fieldLabel: l10n.ns('tpm', 'Dispatch').value('DispatchStart'),
                        labelAlign: 'top',
                        labelSeparator: ' ',
                        width: '100%',
                        labelWidth: 35,
                        layout: {
                            type: 'hbox',
                            align: 'middle',
                            pack: 'center'
                        },
                        items: [{
                            xtype: 'booleancombobox',
                            cls: 'hierarchydetailfield',
                            name: 'IsBeforeStart',
                            store: {
                                type: 'booleanstore',
                                data: [
                                    { id: true, text: l10n.ns('tpm', 'Dispatch').value('Before') },
                                    { id: false, text: l10n.ns('tpm', 'Dispatch').value('After') }
                                ]
                            },
                            flex: 2,
                            style: 'margin-right: 5px;',
                            readOnly: true
                        }, {
                            xtype: 'numberfield',
                            cls: 'hierarchydetailfield',
                            name: 'DaysStart',
                            minValue: 1,
                            allowBlank: false,
                            allowOnlyWhitespace: false,
                            allowDecimals: false,
                            flex: 1,
                            style: 'margin-right: 5px',
                            readOnly: true
                        }, {
                            xtype: 'booleancombobox',
                            cls: 'hierarchydetailfield',
                            name: 'IsDaysStart',
                            store: {
                                type: 'booleanstore',
                                data: [
                                    { id: true, text: l10n.ns('tpm', 'Dispatch').value('Days') },
                                    { id: false, text: l10n.ns('tpm', 'Dispatch').value('Weeks') }
                                ],
                            },
                            flex: 2,
                            readOnly: true
                        }]
                    }, {
                        xtype: 'fieldcontainer',
                        cls: 'dispatchsettingsfieldcontainer',
                        itemId: 'fieldcontainerEnd',
                        fieldLabel: l10n.ns('tpm', 'Dispatch').value('DispatchEnd'),
                        labelAlign: 'top',
                        labelSeparator: ' ',
                        width: '100%',
                        labelWidth: 35,
                        layout: {
                            type: 'hbox',
                            align: 'middle'
                        },
                        style: 'margin-bottom: 0',
                        items: [{
                            xtype: 'booleancombobox',
                            cls: 'hierarchydetailfield',
                            name: 'IsBeforeEnd',
                            store: {
                                type: 'booleanstore',
                                data: [
                                    { id: true, text: l10n.ns('tpm', 'Dispatch').value('Before') },
                                    { id: false, text: l10n.ns('tpm', 'Dispatch').value('After') }
                                ]
                            },
                            flex: 2,
                            style: 'margin-right: 5px',
                            readOnly: true
                        }, {
                            xtype: 'numberfield',
                            cls: 'hierarchydetailfield',
                            name: 'DaysEnd',
                            minValue: 1,
                            allowBlank: false,
                            allowOnlyWhitespace: false,
                            allowDecimals: false,
                            flex: 1,
                            style: 'margin-right: 5px;',
                            readOnly: true
                        }, {
                            xtype: 'booleancombobox',
                            cls: 'hierarchydetailfield',
                            name: 'IsDaysEnd',
                            store: {
                                type: 'booleanstore',
                                data: [
                                    { id: true, text: l10n.ns('tpm', 'Dispatch').value('Days') },
                                    { id: false, text: l10n.ns('tpm', 'Dispatch').value('Weeks') }
                                ]
                            },
                            flex: 2,
                            readOnly: true
                        }]
                    }]
                }, {
                    xtype: 'container',
                    padding: '0 0 7 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [{
                        xtype: 'tbspacer',
                        flex: 1
                    }, {
                        xtype: 'button',
                        text: 'Edit',
                        action: 'UpdateNode',
                        itemId: 'updateNode',
                        glyph: 0xf64f,
                        width: 75,
                        cls: 'hierarchyButton hierarchyButtonEdit',
                        hierarchyOnly: true,
                        needDisable: true,
                    }]
                }],
                listeners: {
                    // Механизм автоматического рассчета правильной высоты панели для нормального отображения скролла
                    boxready: function (panel) {
                        var editorForm = panel.up('editorform');
                        var differenceHeight = editorForm.getHeight() - panel.getHeight() - 20;

                        // Если custompromopanel не умещается
                        if (differenceHeight < 0) {
                            panel.setHeight(panel.getHeight() + differenceHeight);
                            panel.addCls('custompromopanel-clienttree');
                        }
                    }
                }
            }]
        }]
    }]
});