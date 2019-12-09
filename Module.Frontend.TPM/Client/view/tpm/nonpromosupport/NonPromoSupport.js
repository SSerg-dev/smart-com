Ext.define('App.view.tpm.nonpromosupport.NonPromoSupport', {
	extend: 'App.view.core.common.CombinedDirectoryPanel',
	alias: 'widget.nonpromosupport',
	title: l10n.ns('tpm', 'compositePanelTitles').value('NonPromoSupport'),

	customHeaderItems: [
		ResourceMgr.getAdditionalMenu('core').base = {
			glyph: 0xf068,
			text: l10n.ns('core', 'additionalMenu').value('additionalBtn'),

			menu: {
				xtype: 'customheadermenu',
				items: [{
					glyph: 0xf4eb,
					itemId: 'gridsettings',
					text: l10n.ns('core', 'additionalMenu').value('gridSettingsMenuItem'),
					action: 'SaveGridSettings',
					resource: 'Security'
				}]
			}
		},
		ResourceMgr.getAdditionalMenu('core').import = {
			glyph: 0xf21b,
			text: l10n.ns('core', 'additionalMenu').value('importExportBtn'),

			menu: {
				xtype: 'customheadermenu',
				items: [{
					glyph: 0xf21d,
					itemId: 'customexportxlsxbutton',
					exactlyModelCompare: true,
					text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
					action: 'ExportXLSX',
				}]
			}
		}
	],

	dockedItems: [{
		xtype: 'custombigtoolbar',
		dock: 'right'
	}],

	items: [{
		xtype: 'directorygrid',
		itemId: 'datatable',
		editorModel: 'Core.form.EditorDetailWindowModel',
		store: {
			type: 'directorystore',
			model: 'App.model.tpm.nonpromosupport.NonPromoSupport',
			storeId: 'nonpromosupportstore',
			extendedFilter: {
				xclass: 'App.ExtFilterContext',
				supportedModels: [{
					xclass: 'App.ExtSelectionFilterModel',
					model: 'App.model.tpm.nonpromosupport.NonPromoSupport',
					modelId: 'efselectionmodel'
				}, {
					xclass: 'App.ExtTextFilterModel',
					modelId: 'eftextmodel'
				}]
			},
			sorters: [{
				property: 'Number',
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
				text: l10n.ns('tpm', 'PromoSupport').value('Number'),
				dataIndex: 'Number'
			}, {
				text: l10n.ns('tpm', 'PromoSupport').value('ClientTreeFullPathName'),
				dataIndex: 'ClientTreeFullPathName',
				minWidth: 160,
				filter: {
					xtype: 'treefsearchfield',
					trigger2Cls: '',
					selectorWidget: 'clienttree',
					valueField: 'FullPathName',
					displayField: 'FullPathName',
					multiSelect: true,
					operator: 'conts',
					store: {
						model: 'App.model.tpm.clienttree.ClientTree',
						autoLoad: false,
						root: {}
					},
				},
				renderer: function (value) {
					return renderWithDelimiter(value, ' > ', '  ');
				}
			}, {
				text: l10n.ns('tpm', 'NonPromoSupport').value('BrandTech'),
				dataIndex: 'BrandTechName',
				minWidth: 150,
				filter: {
					type: 'search',
					selectorWidget: 'brandtech',
					valueField: 'Name',
					store: {
						type: 'directorystore',
						model: 'App.model.tpm.brandtech.BrandTech',
						extendedFilter: {
							xclass: 'App.ExtFilterContext',
							supportedModels: [{
								xclass: 'App.ExtSelectionFilterModel',
								model: 'App.model.tpm.brandtech.BrandTech',
								modelId: 'efselectionmodel'
							}, {
								xclass: 'App.ExtTextFilterModel',
								modelId: 'eftextmodel'
							}]
						}
					}
				}
			}, {
				text: l10n.ns('tpm', 'PromoSupport').value('BudgetSubItemName'),
				dataIndex: 'NonPromoEquipmentEquipmentType',
				minWidth: 100,
				filter: {
					type: 'search',
					selectorWidget: 'nonpromoequipment',
					valueField: 'EquipmentType',
					store: {
						type: 'directorystore',
						model: 'App.model.tpm.nonpromoequipment.NonPromoEquipment',
						extendedFilter: {
							xclass: 'App.ExtFilterContext',
							supportedModels: [{
								xclass: 'App.ExtSelectionFilterModel',
								model: 'App.model.tpm.nonpromoequipment.NonPromoEquipment',
								modelId: 'efselectionmodel'
							}, {
								xclass: 'App.ExtTextFilterModel',
								modelId: 'eftextmodel'
							}]
						}
					}
				}
			}, {
				text: l10n.ns('tpm', 'PromoSupport').value('PlanQuantity'),
				dataIndex: 'PlanQuantity'
			}, {
				text: l10n.ns('tpm', 'PromoSupport').value('ActualQuantity'),
				dataIndex: 'ActualQuantity'
			}, {
				text: l10n.ns('tpm', 'PromoSupport').value('PlanCostTE'),
				dataIndex: 'PlanCostTE'
			}, {
				text: l10n.ns('tpm', 'PromoSupport').value('ActualCostTE'),
				dataIndex: 'ActualCostTE'
			}, {
				text: l10n.ns('tpm', 'PromoSupport').value('StartDate'),
				dataIndex: 'StartDate',
				renderer: Ext.util.Format.dateRenderer('d.m.Y')
			}, {
				text: l10n.ns('tpm', 'PromoSupport').value('EndDate'),
				dataIndex: 'EndDate',
				renderer: Ext.util.Format.dateRenderer('d.m.Y')
			}]
		}
	}, {
		xtype: 'editabledetailform',
		itemId: 'detailform',
		model: 'App.model.tpm.nonpromosupport.NonPromoSupport',
		items: [{
			xtype: 'numberfield',
			name: 'PlanCostTE',
			fieldLabel: l10n.ns('tpm', 'PromoSupport').value('PlanCostTE')
		}]
	}]
});