Ext.define('App.view.tpm.client.ClientTree', {
    extend: 'App.view.core.common.CombinedDirectoryTreePanel',
    alias: 'widget.clienttree',
    title: l10n.ns('tpm', 'compositePanelTitles').value('ClientTree'),
    customHeaderItems: null,

    systemHeaderItems: [],
    minHeight: 555,
    // true, если на просмотр
    chooseMode: false,
    // выбранный узел (чекнутый) (нужен в промо)
    choosenClientObjectId: null,
    // автозагрузка
    needLoadTree: true,
    // требуется ли скрытие некоторых кнопок
    hideNotHierarchyBtns: false,

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
                    me.fireEvent('applySearch', me.up('clienttree'));
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
                xtype: 'checkbox',
                labelSeparator: '',
                itemId: 'selectAllClientsCheckbox',
                boxLabel: 'Select all',
                labelAlign: 'right',
                style: 'margin-left: 10px',
                listeners: {
                    afterrender: function (checkbox) {
                        var selectionWidget = checkbox.up('clienttree').up('#associatedbudgetsubitemclienttree_clienttree_selectorwindow');
                        if (selectionWidget) {
                            checkbox.show();
                        } else {
                            checkbox.hide();
                        }
                    }
                }
            }, {
                xtype: 'tbspacer',
                flex: 10
            }, {
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
            xtype: 'panel',
            itemId: 'clientTreeSettingsPanel',
            autoScroll: true,
            cls: 'scrollpanel scrollSettingsClientTree',
            height: '100%',
            flex: 1,
            bodyStyle: { "background-color": "#ECEFF1"},
            layout: {
                type: 'vbox',
                align: 'stretch',
                pack: 'start'
            },           
            items: [{
                xtype: 'editorform',
                cls: 'hierarchydetailform',                
                columnsCount: 1,      
                bodyPadding: '0 0 0 0',
                items: [{
                    xtype: 'custompromopanel',
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
                            name: 'GHierarchyCode',
                            width: 280,
                            fieldLabel: l10n.ns('tpm', 'ClientTree').value('GHierarchyCode')
                        }, {
                            xtype: 'singlelinedisplayfield',
                            name: 'DemandCode',
                            width: 280,
                            fieldLabel: l10n.ns('tpm', 'ClientTree').value('DemandCode')
                        }, {
                            xtype: 'singlelinedisplayfield',
                            name: 'IsBaseClient',
                            renderer: App.RenderHelper.getBooleanRenderer('Yes', 'No'),
                            width: 280,
                            fieldLabel: l10n.ns('tpm', 'ClientTree').value('IsBaseClient')
                        }, {
                            xtype: 'singlelinedisplayfield',
                            name: 'IsOnInvoice',
                            renderer: App.RenderHelper.getBooleanRenderer(
                                l10n.ns('tpm', 'InvoiceTypes').value('OnInvoice'),
                                l10n.ns('tpm', 'InvoiceTypes').value('OffInvoice')
                            ),
                            width: 280,
                            fieldLabel: l10n.ns('tpm', 'ClientTree').value('InvoiceType')
                        }, {
                            xtype: 'singlelinedisplayfield',
                            name: 'DMDGroup',
                            width: 280,
                            fieldLabel: l10n.ns('tpm', 'ClientTree').value('DMDGroup')
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
                        }, {
                            xtype: 'fieldcontainer',
                            height: 'auto',
                            width: 'auto',
                            layout: 'fit',
                            minHeight: 132,
                            cls: 'dispatchsettingsfieldcontainer',
                            itemId: 'fieldcontainerAdjustment',
                            labelAlign: 'top',
                            labelSeparator: ' ',
                            width: '100%',
                            layout: {
                                type: 'vbox',
                                align: 'stretch',
                                pack: 'center'
                            },
                            style: 'margin-bottom: 0',
                            items: [{
                                xtype: 'singlelinedisplayfield',
                                width: '100%',
                                fieldLabel: l10n.ns('tpm', 'ClientTree').value('Adjustment'),
                                name: 'Adjustment',
                                width: 280,
                                style: 'margin-top: 10px; margin-bottom: 10px;'
                            }, {
                                xtype: 'sliderfield',
                                minHeight: 40,
                                flex: 1,
                                margin: '0 -7 0 -7',
                                name: 'DeviationCoefficient',
                                minValue: -100,
                                maxValue: 100,
                                cls: 'readonly-adjustment-slider-horz',
                                readOnly: true,
                                style: 'transform: scaleX(-1); opacity: 0.4;',
                                listeners: {
                                    change: function (me, newValue, oldValue) {
                                        var adjustment = this.up('container').down('singlelinedisplayfield[name=Adjustment]');
                                        adjustment.setValue(newValue);
                                    },
                                    render: function (me) {
                                        var adjustment = this.up('container').down('singlelinedisplayfield[name=Adjustment]');
                                        adjustment.setValue(me.getValue());
                                    }
                                }
                            }, {
                                flex: 1,
                                xtype: 'panel',
                                items: [{
                                    xtype: 'label',
                                    html: '\n +100%',
                                    cls: 'slider-left-lable'
                                }, {
                                    xtype: 'label',
                                    html: '\n 0',
                                    cls: 'slider-center-lable'
                                }, {
                                    xtype: 'label',
                                    html: '\n -100%',
                                    cls: 'slider-right-lable'
                                }]
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
                    }]
                }, {
                    xtype: 'custompromopanel',
                    padding: '5 10 9 10',
                    margin: '5 10 10 10',
                    name: 'treeLogoPanel',
                    items: [{
                        xtype: 'container',
                        layout: {
                            type: 'hbox'
                        },
                        defaults: {
                            height: 150
                        },
                        items: [{
                            xtype: 'fieldset',
                            title: l10n.ns('tpm', 'Logo').value('LogoTitleFieldset'),
                            margin: '0 5 0 0',
                            flex: 1,
                            layout: 'vbox',
                            defaults: {
                                width: '100%'
                            },
                            items: [{
                                xtype: 'container',                                
                                flex: 1,
                                layout: {
                                    type: 'hbox',
                                    align: 'stretch',                                    
                                },
                                padding: '50 50 50 50',
                                items: [{
                                    xtype: 'image',
                                    itemId: 'logoImage',
                                    //maxHeight: 90,
                                    padding: '5 5 5 5',
                                    src: null // Устанавливается при клике на узел
                                }]
                            }, {
                                xtype: 'container',
                                maring: 0,
                                height: 27,
                                layout: 'hbox',
                                items: [{
                                    xtype: 'singlelinedisplayfield',
                                    itemId: 'attachFileName',
                                    value: 'Attach your file',
                                    width: '100%',
                                    flex: 1,
                                    style: 'text-decoration: underline',
                                    hierarchyOnly: true,              
                                    action: 'UploadLogoFile',
                                }, {
                                    xtype: 'button',
                                    itemId: 'attachFile',
                                    text: 'Attach File',
                                    cls: 'attachfile-button',
                                    minWidth: 85,
                                    margin: '0 0 0 4',
                                    hierarchyOnly: true,
                                    action: 'UploadLogoFile',
                                }, {
                                    xtype: 'button',
                                    itemId: 'deleteAttachFile',
                                    text: 'Delete',
                                    cls: 'deletefile-button',
                                    minWidth: 60,
                                    margin: '0 0 0 4',
                                    hierarchyOnly: true,
                                    action: 'DeleteLogo'
                                }]
                            }]
                        }, {
                            xtype: 'fieldset',
                            title: l10n.ns('tpm', 'Logo').value('DescriptionTitleFieldset'),
                            margin: '0 0 0 5',
                            padding: '0 10 5 10',
                            flex: 1,
                            html: l10n.ns('tpm', 'Logo').value('DescriptionText')
                        }]
                    }]
                }]
            }]  
        }] 
    }]
});