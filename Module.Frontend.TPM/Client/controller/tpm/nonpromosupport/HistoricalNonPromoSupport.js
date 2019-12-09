Ext.define('App.controller.tpm.nonpromosupport.HistoricalNonPromoSupport', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
			component: {
				'historicalnonpromosupport directorygrid': {
					load: this.onGridStoreLoad
				},
				'historicalnonpromosupport directorygrid': {
					itemdblclick: this.onDetailButtonClick,
					selectionchange: this.onGridSelectionChange,
					afterrender: this.onGridAfterrender,
					extfilterchange: this.onExtFilterChange
				},
				'historicalnonpromosupport #datatable': {
					activate: this.onActivateCard
				},
				'historicalnonpromosupport #detailform': {
					activate: this.onActivateCard
				},
				'historicalnonpromosupport #detail': {
					click: this.onDetailButtonClick
				},
				'historicalnonpromosupport #table': {
					click: this.onTableButtonClick
				},
				'historicalnonpromosupport #prev': {
					click: this.onPrevButtonClick
				},
				'historicalnonpromosupport #next': {
					click: this.onNextButtonClick
				},
				'historicalnonpromosupport #extfilterbutton': {
					click: this.onFilterButtonClick
				},
				'historicalnonpromosupport #refresh': {
					click: this.onRefreshButtonClick
				},
				'historicalnonpromosupport #close': {
					click: this.onCloseButtonClick
				}
			}
        });
    }
});
