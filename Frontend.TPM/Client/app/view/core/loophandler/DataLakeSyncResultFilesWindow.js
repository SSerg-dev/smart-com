Ext.define('App.view.core.loophandler.DataLakeSyncResultFilesWindow', {
	extend: 'App.view.core.base.BaseReviewWindow',
	alias: 'widget.datalakesyncresultfileswindow',
	resizeHandles: 'w e',

	title: l10n.ns('core').value('dataLakeSyncResultFilesWindow'),

	width: 400,
	height: null,
	minWidth: 300,
	minHeight: 150,
	maxHeight: 500,

	items: [{
		xtype: 'form',
		itemId: 'datalakesyncresultfilesform',
		ui: 'detailform-panel',

		bodyPadding: '10 10 0 10',

		layout: {
			type: 'vbox',
			align: 'stretch',
			pack: 'center'
		},

		defaults: {
			xtype: 'singlelinedisplayfield',
			ui: 'detail-form-field',
			labelAlign: 'left',
			labelWidth: 200,
			labelSeparator: '',
			labelPad: 0
		},

		items: []
	}]
});