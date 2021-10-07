Ext.define('App.view.core.rpa.RPAForm', {
	extend: 'Ext.form.Panel',
	alias: 'widget.rpaform',	
	
	items: [{
		xtype: 'editorform',
		columnsCount: 1,
		layout: {
            type: 'vbox',
            align: 'stretch'
        },
		scroll: true,
        overflowY: 'scroll',
        height: '100%',
		items: [{
			xtype: 'panel',
			height: '100%',
			autoScroll: true,
			flex: 1,
				layout: {
					type: 'vbox',
					align: 'stretch',
					pack: 'start'
				},
				items: [{
					xtype: 'fieldset',
					title: 'Handlers and templates',					
					items: [{
						layout: 'vbox',
						xtype:"container",
						layout: {
							type: 'vbox',
							align: 'stretch'
						},
						items:[{
						xtype: 'combobox',
						name: 'HandlerName',
						fieldLabel: l10n.ns('core', 'RPA').value('HandlerName'),
						valueField: 'Name',
						displayField: 'Name',           
						entityType: 'RPASetting',
						allowBlank: false,
						allowOnlyWhitespace: false,
						store: {
							type: 'simplestore',
							autoLoad: false,
							model: 'App.model.core.rpasetting.RPASetting',
							extendedFilter: {
								xclass: 'App.ExtFilterContext',
								supportedModels: [{
									xclass: 'App.ExtSelectionFilterModel',
									model: 'App.model.core.rpasetting.RPASetting',
									modelId: 'efselectionmodel'
								}]
							 }
						},
						listeners: {
							select: function(combo, record){								                            
								let paramFieldSet = Ext.getCmp('params');
								if(paramFieldSet['items']['items'].length>0)
								{
									paramFieldSet.removeAll();
								}
								if(isJsonValid(record[0].data['Json'])){
									let templateLink = Ext.getCmp('templateLink');
									templateLink.setVisible(true);
									templateLink.getEl().on('click', function(e){
										e.preventDefault(true);
										var url = Ext.String.format("odata/{0}/{1}", 'RPAs', 'DownloadTemplateXLSX');
										Ext.Ajax.request({   
											method: 'POST',   
											url: url,
											params: {handlerId: Ext.JSON.encode(record[0].data['Id'])},
											success: function (data) {		
												var filename = JSON.parse(data.responseText).value;																							
												var href = Ext.String.format('api/File/{0}?{1}={2}', 'ExportDownload', 'filename', filename);
        										var aLink = document.createElement('a');
        										aLink.download = filename;
												aLink.href = href;
												document.body.appendChild(aLink);
												aLink.click();
												document.body.removeChild(aLink)
											},
											fail: function (data) {
												
											}
										})
									});
									const parametrs = JSON.parse(record[0].data['Json'])["parametrs"];
									if(parametrs) {                           
											Ext.Array.each(parametrs,function(element,index){
												let paramField ={
													xtype: 'textfield',
													name: element["name"],
													fieldLabel: element["name"],
													value: element["value"],
													id: element["name"],
													width: 400
												};
												paramFieldSet.add(paramField);
											});
											paramFieldSet.setVisible(true);
										}
								}
								else{
									Ext.MessageBox.show({
										title: 'Error',
										msg: 'Wrong Json format. Please check RPA setting or parameters is empty.',
										buttons: Ext.MessageBox.OK,
										icon: Ext.MessageBox.ERROR,
									});
								}
							},
							afterrender: function(combo) {
								store = combo.store;
								store.on('load', function(res){                                
									if(res.data.items.length===0){                                    
										combo.setValue("This role does not have a configured handler");
										Ext.getCmp('eventfile').setDisabled(true);
										Ext.ComponentQuery.query('#saveRPAForm')[0].setDisabled(true);
									}                                
								})
							}
						},
						mapping: [{
							from: 'Name',
							to: 'HandlerName'
						}]
					}, {
						xtype: 'label',
						glyph: 0xf21d,
						html: '<a href="#">Import template XLSX</a>',
						id: "templateLink",
						hidden: true,
						style: {
							'text-align':'right'
						}
					}]
				}]
				}, {
					xtype: 'fieldset',
					title: 'Parametrs',
					layout: {
						type: 'vbox',
						align: 'stretch',
					},
					id: "params"
				}, {
					xtype: 'filefield',
					name: 'File',
					id: 'eventfile',
					msgTarget: 'side',
					buttonText: l10n.ns('core', 'buttons').value('browse'),
					forceValidation: true,
					allowOnlyWhitespace: false,
					allowBlank: false,
					fieldLabel: l10n.ns('core').value('uploadFileLabelText'),
					vtype: 'filePass',
					ui: 'default',
					labelWidth: '10%'
				}]
		}]
	}]
});

function isJsonValid(str){
	try {
		JSON.parse(str);
	} catch (e) {
		return false;
	}
	return true;
}