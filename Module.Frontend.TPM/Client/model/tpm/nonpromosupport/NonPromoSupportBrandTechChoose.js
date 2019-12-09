Ext.define('App.view.tpm.nonpromosupport.NonPromoSupportBrandTechChoose', {
	extend: 'App.view.core.base.BaseModalWindow',
	alias: 'widget.nonpromosupportbrandtechchoose',
    title: l10n.ns('tpm', 'compositePanelTitles').value('BrandTech'),

	width: "95%",
	height: "95%",
	minWidth: 800,
	minHeight: 600,

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
			xtype: 'brandtech',
			flex: 1,

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
		text: l10n.ns('tpm', 'NonPromoSupport').value('brandTechOk'),
		ui: 'green-button-footer-toolbar',
		itemId: 'ok'
	}]
});