Ext.define('App.view.tpm.promo.ApprovalHistory', {
	extend: 'Ext.container.Container',
	alias: 'widget.approvalhistory',

	name: 'changes',
	itemId: 'changes',
	isLoaded: false,
	isNonego: false,
	promoStatus: 'Draft',
	historyArray: [],
	statusColors: {
		Approved: '#81C784',
		Cancelled: '#ffffff',
		Closed: '#A1887F',
		Deleted: '#e57373',
		Draft: '#90A4AE',
		DraftPublished: '#7986CB',
		Finished: '#4DB6AC',
		OnApproval: '#FFD54F',
		Planned: '#DCE775',
		Started: '#64B5F6',
	},
	historyTpl: Ext.create('App.view.tpm.common.approvalHistoryTpl').formatTpl,
	layout: 'fit',
	items: [{
		xtype: 'fieldset',
		//title: l10n.ns('tpm', 'promoMainTab').value('changeStatusHistory'),
		layout: 'fit',
		cls: 'approval-history-fieldset',

		items: [{
			xtype: 'panel',
			overflowY: 'scroll',
			cls: 'scrollpanel',
			dockedItems: [{
				xtype: 'toolbar',
				dock: 'top',
				cls: 'approval-status-toolbar',
				items: [{
					xtype: 'button',
					glyph: 0xf1a9,
					text: 'Workflow Process',
					cls: 'approval-status-history-btn selected',
					id: 'workflowBtn',
					active: true,
					listeners: {
						click: function (btn) {
							var panel = btn.up('panel');
							var container = panel.up('container[name=changes]');
							var historyBtn = panel.down('button[id=historyBtn]');
							var dateOfChangeLable = panel.down('label');
							var reverseBtn = panel.down('button[id=reverseBtn]');
							var promo = null;
							var onApprovalState = null;
							var promoStatusName = 'Draft';

							if (historyBtn) {
								historyBtn.removeCls('selected');
								historyBtn.active = false;
							}
							btn.addClass('selected');
							btn.active = true;
							dateOfChangeLable.hide();
							reverseBtn.hide();

							var tpl = Ext.create('App.view.tpm.common.approvalStatusStateTpl').formatTpl;
							var itemsArray = [];

							var panelWidthRatio = panel.getWidth() / 1160;
							//Small sizes correction:
							if (panelWidthRatio * 130 < 120) {
								panelWidthRatio = panelWidthRatio * 0.96;
								if (panelWidthRatio * 130 < 90) {
									panelWidthRatio = panelWidthRatio * 0.93;
								}
							}
							var panelHeightRatio = (panel.getWidth() / 1160) * (100 / 130);

							if (container.historyArray != null) {
								if (container.historyArray.length == 0) {
									var promoeditorcustom = container.up('promoeditorcustom');
                                    promo = promoeditorcustom.model ? promoeditorcustom.model.data : promoeditorcustom.assignedRecord ? promoeditorcustom.assignedRecord.data : null;
								} else {
									promo = container.historyArray[0].Promo;
								}
								promoStatusName = promo.PromoStatus == undefined ? promo.PromoStatusSystemName : promo.PromoStatus.SystemName

                                if (promo) {
                                    if (promo.IsCMManagerApproved == true && (promo.IsDemandPlanningApproved == false || promo.IsDemandPlanningApproved == null) && promo.IsDemandFinanceApproved == true)
                                        onApprovalState = 'DemandPlanningNonego';
                                    else if ((promo.IsCMManagerApproved == false || promo.IsCMManagerApproved == null) && (promo.IsDemandPlanningApproved == false || promo.IsDemandPlanningApproved == null) && (promo.IsDemandFinanceApproved == false || promo.IsDemandFinanceApproved == null))
                                        onApprovalState = 'CMManager';
                                    else if (promo.IsCMManagerApproved == true && (promo.IsDemandPlanningApproved == false || promo.IsDemandPlanningApproved == null) && (promo.IsDemandFinanceApproved == false || promo.IsDemandFinanceApproved == null))
                                        onApprovalState = 'DemandPlanning';
                                    else if (promo.IsCMManagerApproved == true && promo.IsDemandPlanningApproved == true && (promo.IsDemandFinanceApproved == false || promo.IsDemandFinanceApproved == null))
                                        onApprovalState = 'DemandFinance';
                                }
							}

                            if (promo) {
                                var settings = {
                                    currentWidthRatio: panelWidthRatio,
                                    currentHeightRatio: panelHeightRatio,
                                    currentHeight: panel.body.getHeight(),
                                    status: promoStatusName,
                                    onApprovalState: onApprovalState,
                                    isNonego: container.isNonego == null ? false : container.isNonego,
                                    statusHistory: container.historyArray == null ? [] : container.historyArray,
                                    statusColors: container.statusColors
                                }
                            }

							itemsArray.push({
								html: tpl.apply(settings),
							});

							panel.removeAll();
							panel.add(itemsArray);
							panel.doLayout();

							var elementsWithTips = Ext.select('div[toolTip*=]');

							elementsWithTips.elements.forEach(function (el) {
								var me = el;
								me.fieldLabelTip = Ext.create('Ext.tip.ToolTip', {
									target: me,
									preventLoseFocus: true,
									trackMouse: true,
									html: me.getAttribute('tooltip'),
									dismissDelay: 15000
								});
							})
						},
						scope: this
					}
				}, {
					xtype: 'button',
					glyph: 0xf2da,
					text: 'Status History',
					cls: 'approval-status-history-btn',
					id: 'historyBtn',
					active: false,
					listeners: {
						click: function (btn) {
							var panel = btn.up('panel');
							var container = panel.up('container[name=changes]');
							var workflowBtn = panel.down('button[id=workflowBtn]');
							var dateOfChangeLable = panel.down('label');
							var reverseBtn = panel.down('button[id=reverseBtn]');

							if (workflowBtn) {
								workflowBtn.removeCls('selected');
								workflowBtn.active = false;
							}
							btn.addClass('selected');
							btn.active = true;
							dateOfChangeLable.show();
							reverseBtn.show();

							reverseBtn.ascendDate = true;
							reverseBtn.setGlyph(0xf4bd);

							var tpl = Ext.create('App.view.tpm.common.approvalHistoryTpl').formatTpl;
							var tplDecline = Ext.create('App.view.tpm.common.approvalHistoryDeclineTpl').formatTpl;

							if (container.historyArray == null) {
								panel.removeAll();
								return;
							}

							var itemsArray = [];
							for (var i = 0; i < container.historyArray.length; i++) {
								if (i == container.historyArray.length - 1) {
									container.historyArray[i].IsLast = true;
								} else {
									container.historyArray[i].IsLast = false;
								}

								// если есть Comment, то значит промо было оклонено
								if (container.historyArray[i].RejectReasonId !== null) {
									itemsArray.push({
										html: tplDecline.apply(container.historyArray[i]),
									});
								}
								else {
									itemsArray.push({
										html: tpl.apply(container.historyArray[i]),
									});
								}
							}

							panel.removeAll();
							panel.add(itemsArray);
							panel.doLayout();
						},
						scope: this
					},
				}, '->', {
					xtype: 'label',
					text: l10n.ns('tpm', 'text').value('dateOfChange'),
					cls: 'approval-history-reverse-lable'
				}, {
					xtype: 'button',
					text: '',
					glyph: 0xf4bd,
					ascendDate: true,
					cls: 'approval-history-reverse-btn',
					id: 'reverseBtn',
					reversed: false,
					listeners: {
						click: function (btn) {
							var panel = btn.up('panel');
							var container = panel.up('container[name=changes]');

							if (btn.ascendDate) {
								container.historyArray.reverse();
							}
							
							var itemsArray = [];
							for (var i = 0; i < container.historyArray.length; i++) {
								if (i == container.historyArray.length - 1) {
									container.historyArray[i].IsLast = true;
								} else {
									container.historyArray[i].IsLast = false;
								}

								itemsArray.push({
									html: container.historyTpl.apply(container.historyArray[i]),
								});
							}

							panel.removeAll();
							panel.add(itemsArray);
							panel.doLayout();

							if (btn.ascendDate) {
								container.historyArray.reverse();
							}
							btn.ascendDate = !btn.ascendDate;

							if (btn.ascendDate) {
								btn.setGlyph(0xf4bd);
							} else {
								btn.setGlyph(0xf4bc);
							}
						},
						scope: this
					}
				}]
			}],
			items: [{}]
		}],
		listeners: {
			resize: function (fieldset, width, height, oldWidth, oldHeight, eOpts) {
				var container = fieldset.up('container[name=changes]');
				//Update height
				var parentHeight = container.up().getHeight();
				fieldset.setHeight(parentHeight - 13);
				//Update tpl
				if (container.historyArray.length > 0) {
					var historyBtn = fieldset.down('button[id=historyBtn]');
					var workflowBtn = fieldset.down('button[id=workflowBtn]');

					if (historyBtn.active) {
						historyBtn.fireEvent('click', historyBtn);
					} else if (workflowBtn.active) {
						workflowBtn.fireEvent('click', workflowBtn);
					}
				}

				if (width < 1000) {
					var discriptions = Ext.select('div[id=discription]');

					discriptions.elements.forEach(function (item) {
						Ext.fly(item).update('...');
					});
				}
			}
		},
	}]
});
