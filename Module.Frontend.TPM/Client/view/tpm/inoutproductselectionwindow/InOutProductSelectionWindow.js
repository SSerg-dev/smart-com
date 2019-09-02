Ext.define('App.view.tpm.inoutselectionproductwindow.InOutProductSelectionWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.inoutselectionproductwindow',
    title: l10n.ns('tpm', 'compositePanelTitles').value('InOutProductSelectionWindow'),
    width: "95%",
    height: "95%",
    minWidth: 800,
	minHeight: 600,

	tools: [{
		xtype: 'button',
		itemId: 'dateFilter',
		text: '00.00.0000',
		cls: 'custom-promo-date-button',
		glyph: 0xf0f6
	}],

    items: [{
        xtype: 'container',
        layout: {
            type: 'hbox',
        },
        defaults: {
            height: '100%',
            style: 'box-shadow: none'
        },
        items: [{
            xtype: 'container',
            itemId: 'InOutProductSelectionWindowProductTreeGridContainer',
            choosenProductObjectId: [],
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            style: 'background-color: white',
            flex: 1,
			margin: '0 5 0 0',
            items: [{
                xtype: 'customtoptreetoolbar',
				dock: 'top',
                items: [{
                    xtype: 'container',
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
                            me.fireEvent('applySearch', me.up('#InOutProductSelectionWindowProductTreeGridContainer').down('producttreegrid'));
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
                    }]
                }]
            }, {
                xtype: 'producttreegrid',
                cls: 'template-tree scrollpanel in-out-product-selection-window-product-tree-grid',
                flex: 1,
                minWidth: 263,
            }]
        }, {
            xtype: 'product',
            assortmentMatrixStore: null,
            flex: 2,
            //убираем тулбар справа и кнопки сверху
            dockedItems: [{
                xtype: 'custombigtoolbar',
                dock: 'right',
                items: [{
                    xtype: 'widthexpandbutton',
                    ui: 'fill-gray-button-toolbar',
                    text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
                    glyph: 0xf13d,
                    glyph1: 0xf13e,
                    target: function () {
                        return this.up('toolbar');
                    },
                }, {
                    itemId: 'excludeassortmentmatrixproductsbutton',
                    glyph: 0xf755,
                    enableToggle: true,
                    text: l10n.ns('tpm', 'InOutProductSelectionWindow').value('excludeassortmentmatrixproductsbutton'),
                    tooltip: l10n.ns('tpm', 'InOutProductSelectionWindow').value('excludeassortmentmatrixproductsbutton'),
                    checkedProducts: []
                }, '-', {
                    itemId: 'extfilterbutton',
                    glyph: 0xf349,
                    text: l10n.ns('core', 'toptoolbar').value('filterButtonText'),
                    tooltip: l10n.ns('core', 'toptoolbar').value('filterButtonText')
                }, '-', '->', '-', {
                    itemId: 'extfilterclearbutton',
                    ui: 'blue-button-toolbar',
                    disabled: true,
                    glyph: 0xf232,
                    text: l10n.ns('core', 'filter').value('filterEmptyStatus'),
                    tooltip: l10n.ns('core', 'filter').value('filterEmptyStatus'),
                    overCls: '',
                    style: {
                        'cursor': 'default'
                    }
                }]
            }],
            systemHeaderItems: [],
            customHeaderItems: [],
        }]
    }],

    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'close'
    }, {
        text: l10n.ns('tpm', 'InOutProductSelectionWindow').value('ok'),
        ui: 'green-button-footer-toolbar',
        itemId: 'ok'
    }]
})
