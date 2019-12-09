Ext.define('App.controller.tpm.nonpromoequipment.HistoricalNonPromoEquipment', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
			component: {
				'historicalnonpromoequipment directorygrid': {
					load: this.onGridStoreLoad
				},
				'historicalnonpromoequipment directorygrid': {
					itemdblclick: this.onDetailButtonClick,
					selectionchange: this.onGridSelectionChange,
					afterrender: this.onGridAfterrender,
					extfilterchange: this.onExtFilterChange
				},
				'historicalnonpromoequipment #datatable': {
					activate: this.onActivateCard
				},
				'historicalnonpromoequipment #detailform': {
					activate: this.onActivateCard
				},
				'historicalnonpromoequipment #detail': {
					click: this.onDetailButtonClick
				},
				'historicalnonpromoequipment #table': {
					click: this.onTableButtonClick
				},
				'historicalnonpromoequipment #prev': {
					click: this.onPrevButtonClick
				},
				'historicalnonpromoequipment #next': {
					click: this.onNextButtonClick
				},
				'historicalnonpromoequipment #extfilterbutton': {
					click: this.onFilterButtonClick
				},
				'historicalnonpromoequipment #refresh': {
					click: this.onRefreshButtonClick
				},
				'historicalnonpromoequipment #close': {
					click: this.onCloseButtonClick
				}
			}
        });
    }
});
