Ext.define('App.controller.tpm.nonpromosupport.NonPromoSupportBrandTechChoose', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
			component: {
				'nonpromosupportbrandtechchoose #ok': {
					click: this.onOkButtonClick
				},
				'nonpromosupportbrandtechchoose[isSearch!=true] directorygrid': {
					load: this.onGridStoreLoad,
				},
				'nonpromosupportbrandtechchoose directorygrid': {
					selectionchange: this.onGridSelectionChange,
					afterrender: this.onGridAfterrender,
					extfilterchange: this.onExtFilterChange
				},

				'nonpromosupportbrandtechchoose #extfilterbutton': {
					click: this.onFilterButtonClick
				},
				'nonpromosupportbrandtechchoose #historybutton': {
					click: this.onHistoryButtonClick
				},
				'nonpromosupportbrandtechchoose #refresh': {
					click: this.onRefreshButtonClick
				},
				'nonpromosupportbrandtechchoose #close': {
					click: this.onCloseButtonClick
				}
            }
        });
	},

	onOkButtonClick: function (button) {
		var editor = Ext.ComponentQuery.query('customnonpromosupporteditor')[0];
		var editorBrandTech = editor.down('nonpromosupportbrandtech');
		var brandBtn = editorBrandTech.down('singlelinedisplayfield[name=Brand]');
		var techBtn = editorBrandTech.down('singlelinedisplayfield[name=Technology]');


		var brandTech = button.up('window').down('brandtech');
		var brandTechGrid = brandTech.down('grid');
		var selectionModel = brandTechGrid.getSelectionModel();
		var record = selectionModel.getSelection()[0];

		brandBtn.setValue(record.get('BrandName'));
		techBtn.setValue(record.get('TechnologyName'));
		editor.brandTechId = record.get('Id');
		button.up('window').close();
	}
});
