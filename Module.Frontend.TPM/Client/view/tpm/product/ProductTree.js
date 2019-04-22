Ext.define('App.view.tpm.product.ProductTree', {
    extend: 'App.view.core.common.CombinedDirectoryTreePanel',
    alias: 'widget.producttree',
    title: l10n.ns('tpm', 'compositePanelTitles').value('ProductTree'),
    customHeaderItems: null,

    name: 'initialproducttree',
    systemHeaderItems: [],
    minHeight: 550,

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
                    me.fireEvent('applySearch');
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
            minWidth: 263,
            flex: 1,
            xtype: 'editorform',
            cls: 'custom-tree-editor',
            cls: 'hierarchydetailform',
            layout: 'fit',
            header: null,
            columnsCount: 1,
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
                    }]
                }, {
                    xtype: 'container',
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
                        margin: '10 0 10 0'
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
                    items: [{
                        xtype: 'container',
                        name: 'filterProductContainer',
                        autoScroll: true,
                        height: 185,
                    }],
                    margin: '0 0 10 0'
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
            }]
        }]
    }]
});