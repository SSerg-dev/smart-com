Ext.define('App.view.tpm.product.ProductTree', {
    extend: 'App.view.core.common.CombinedDirectoryTreePanel',
    alias: 'widget.producttree',
    title: l10n.ns('tpm', 'compositePanelTitles').value('ProductTree'),
    customHeaderItems: null,

    name: 'initialproducttree',
    systemHeaderItems: [],
    minHeight: 550,

    // true, если на просмотр
    chooseMode: false,
    // выбранный узел (чекнутый) (нужен в промо)
    choosenClientObjectId: [],
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
                itemId: 'productsSearchTrigger',
                hideLabel: true,
                editable: true,
                cls: 'tree-search-text-def',
                maxWidth: 300,
                width: 300,
                onTriggerClick: function () {
                    var me = this;
                    me.setRawValue('Product search');
                    me.addClass('tree-search-text-def');
                    me.fireEvent('applySearch', me.up('producttree'));
                    me.triggerBlur();
                    me.blur();
                },
                listeners: {
                    afterrender: function (field) {
                        field.setRawValue('Product search');
                    },
                    focus: function (field) {
                        if (field.getRawValue() == 'Product search') {
                            field.setRawValue('');
                            field.removeCls('tree-search-text-def');
                        }
                    },
                    blur: function (field) {
                        if (field.getRawValue() == '') {
                            field.setRawValue('Product search');
                            field.addClass('tree-search-text-def');
                        }
                    }
                }
            }, {
                xtype: 'tbspacer',
                flex: 10
            },/* {
                itemId: 'moveNode',
                glyph: 0xf252,
                text: 'Move',
                tooltip: 'Move',
                hierarchyOnly: true,
                needDisable: true
            },*/ {
                xtype: 'button',
                cls: 'hierarchyButton hierarchyButtonAdd',
                action: 'Post',
                itemId: 'addNode',
                glyph: 0xf415,
                //text: 'Add Node',
                tooltip: 'Add Node',
                hierarchyOnly: true,
                needDisable: true
            }, {
                xtype: 'button',
                cls: 'hierarchyButton hierarchyButtonDelete',
                action: 'Delete',
                itemId: 'deleteNode',
                glyph: 0xf5ad,
                //text: 'Delete Node',
                tooltip: 'Delete Node',
                hierarchyOnly: true,
                needDisable: true
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
                itemId: 'productList',
                hidden: true,
                text: l10n.ns('tpm', 'button').value('ProductList'),
                tooltip: 'Product list',
                icon: '/'
            }]
        }]
    }],

    items: [{
        xtype: 'container',
        cls: 'combined-directory-tree-panel-items-container',
        layout: 'hbox',
        items: [{
            xtype: 'producttreegrid',
            height: '100%',
            flex: 1,
            minWidth: 263,
        }, {
            xtype: 'splitter',
            itemId: 'splitter_2',
            cls: 'custom-tree-panel-splitter',
            collapseOnDblClick: false,
            listeners: {
                dblclick: {
                    fn: function (event, el) {
                        var cmp = Ext.ComponentQuery.query('splitter[itemId=splitter_2]')[0];
                        cmp.tracker.getPrevCmp().flex = 1;
                        cmp.tracker.getNextCmp().flex = 1;
                        cmp.ownerCt.updateLayout();
                    },
                    element: 'el'
                }
            }
        }, {
            xtype: 'panel',
            autoScroll: true,
            cls: 'scrollpanel scrollSettingsClientTree',
            //minWidth: 700,            
            height: '100%',
            flex: 1,
            bodyStyle: { "background-color": "#ECEFF1" },
            layout: {
                type: 'vbox',
                align: 'stretch',
                pack: 'start'
            },
            items: [{
                minWidth: 263,                
                xtype: 'editorform',
                cls: 'custom-tree-editor',
                cls: 'hierarchydetailform',                
                layout: 'fit',
                header: null,
                columnsCount: 1,
                bodyPadding: '0 0 0 0',
                margin: 0,
                items: [{
                    xtype: 'custompromopanel',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [{
                        xtype: 'fieldset',
                        title: 'Parameters',
                        margin: '0 0 10 0',
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            padding: '0 3 0 3',
                        },
                        items: [{
                            xtype: 'singlelinedisplayfield',
                            name: 'ObjectId',
                            width: 350,
                            fieldLabel: l10n.ns('tpm', 'ProductTree').value('ProductHierarchyCode'),
                        }, {
                            xtype: 'singlelinedisplayfield',
                            name: 'Type',
                            width: 280,
                            fieldLabel: l10n.ns('tpm', 'ProductTree').value('Type'),
                        }, {
                            xtype: 'singlelinedisplayfield',
                            name: 'Name',
                            width: 280,
                            fieldLabel: l10n.ns('tpm', 'ProductTree').value('Name'),
                        }, {
                            xtype: 'singlelinedisplayfield',
                            name: 'Abbreviation',
                            width: 280,
                            fieldLabel: l10n.ns('tpm', 'ProductTree').value('Abbreviation')
                        }, {
                            xtype: 'singlelinedisplayfield',
                            name: 'NodePriority',
                            width: 280,
                            fieldLabel: l10n.ns('tpm', 'ProductTree').value('NodePriority')
                        }
                        ]
                    }, {
                        xtype: 'container',
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
                            text: 'Edit',
                            action: 'UpdateNode',
                            itemId: 'updateNode',
                            glyph: 0xf64f,
                            width: 75,
                            cls: 'hierarchyButton hierarchyButtonEdit',
                            hierarchyOnly: true,
                            needDisable: true,
                            margin: '0 0 10 0'
                        }]
                    }]
                }, {
                    xtype: 'custompromopanel',
                    cls: 'custom-promo-panel custom-promo-panel-advanced-search-filter',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [{
                        xtype: 'fieldset',
                        title: 'Advanced Search Filter',
                        layout: 'fit',
                        items: [{
                            xtype: 'container',
                            name: 'filterProductContainer',
                            flex: 1,
                            cls: 'scrollpanel',
                            autoScroll: false,
                            height: 160,
                            overflowY: 'auto',
                            overflowX: false,
                        }],
                        margin: '0 0 10 0',
                    }, {
                        xtype: 'tbspacer',
                        flex: 1
                    }, {
                        xtype: 'container',
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
                            cls: 'hierarchyButton hierarchyButtonList hierarchyButtonNoImage',
                            itemId: 'filteredProductList',
                            text: l10n.ns('tpm', 'button').value('FilteredProductList'),
                            tooltip: 'Filtered Products',
                            glyph: 0xf232,
                            width: 140
                        }, {
                            xtype: 'tbspacer',
                            width: 4
                        }, {
                            xtype: 'button',
                            text: 'Edit',
                            action: 'UpdateNode',
                            itemId: 'editFilter',
                            glyph: 0xf64f,
                            width: 75,
                            cls: 'hierarchyButton',
                            hierarchyOnly: true,
                            needDisable: true,
                            margin: '0 0 10 0'
                        }]
                    }]
                }, {
                    xtype: 'custompromopanel',
                    padding: '5 10 9 10',
                    margin: '0 10 10 10',
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
                                    action: 'DeleteLogo',
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