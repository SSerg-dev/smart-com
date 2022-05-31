Ext.define('App.view.tpm.common.approvalStatusStateTpl', {
    formatTpl: new Ext.XTemplate([
        '<div class="approval-status-state-pathLine">',
			'<svg height="{[values.svgHeight]}" width="{[values.svgWidth]}">',
				/*OnApproval -> DraftPublished*/
				'<polyline points="{[this.getLinePoints("FromOnApproval", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" style="fill:none; stroke:#829cb8; stroke-width:1" />',
				'<polygon points="{[this.getPointerPoints("FromOnApproval", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" fill="#829cb8" stroke-width="0" />',
				/*Approved -> OnApproval*/
				'<polyline points="{[this.getLinePoints("FromApproved", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" style="fill:none; stroke:#829cb8; stroke-width:1" />',
				'<polygon points="{[this.getPointerPoints("FromApproved", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" fill="#829cb8" stroke-width="0" />',
				/*Planned -> OnApproval*/
				'<polyline points="{[this.getLinePoints("FromPlanned", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" style="fill:none; stroke:#829cb8; stroke-width:1" />',
				'<polygon points="{[this.getPointerPoints("FromPlanned", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" fill="#829cb8" stroke-width="0" />',
				/*ToDeleted*/
				'<polyline points="{[this.getLinePoints("ToDeleted", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" style="fill:none; stroke:#E57373; stroke-width:1" />',
				'<polygon points="{[this.getPointerPoints("ToDeleted", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" fill="#E57373" stroke-width="0" />',
				/*ToApproval*/
				'<polyline points="{[this.getLinePoints("ToApproval", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" style="fill:none; stroke:#829cb8; stroke-width:1" />',
				'<polygon points="{[this.getPointerPoints("ToApprovalFirst", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" fill="#829cb8" stroke-width="0" />',
				'<polygon points="{[this.getPointerPoints("ToApprovalSecond", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" fill="#829cb8" stroke-width="0" />',
				'<polygon points="{[this.getPointerPoints("ToApprovalThird", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" fill="#829cb8" stroke-width="0" />',
				/*ToCanceled*/
				'<polyline points="{[this.getLinePoints("ToCanceled", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" style="fill:none; stroke:#829cb8; stroke-width:1" />',
				'<polygon points="{[this.getPointerPoints("ToCanceled", values.currentHeightRatio, values.currentWidthRatio, values.currentHeight)]}" fill="#829cb8" stroke-width="0" />',
			'</svg>',
		'</div>',

        '<div class="approval-status-selector" style="{[this.getStatusSelectorStyle(values.status, values.currentWidthRatio, values.currentHeightRatio, values.currentHeight)]}"><div class = "approval-status-selector-corner"></div></div>',
		'<div class="approval-state-selector" style="{[this.getStateSelectorStyle(values.status, values.isNonego, values.onApprovalState, values.currentWidthRatio, values.currentHeightRatio)]}"><div class = "approval-status-selector-corner"></div></div>',

		/* Draft */
		'<div class="approval-status-state-boxContainer">',
			'<div class="approval-status-state-box first {[this.getStatusBoxClass(values.status, "Draft")]}" style="{[this.getStatusBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">',
				'<div class="approval-status-state-boxDate">{[this.getDate(values.status, values.statusHistory, "Draft")]}</div>', 
				'<div class="approval-status-state-boxLine draft" style="{[this.getStatusColor(values.statusColors, "Draft")]}">',
					'<b>Draft</b><br>(not published)',
					'<div class="approval-status-state-arrow">',
						'<svg height="10" width="10">',
							'<polygon points="2.5,0 7.5,5 2.5,10" fill="#829cb8" stroke-width="0" />',
						'</svg>',	
					'</div >',
				'</div>',
				'<div toolTip="Creation of a promo in a draft version without displaying in the official calendar." id="discription" class="approval-status-state-boxDiscription" style="{[this.getDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">Creation of a promo in a draft version without displaying in the...</div>',
			'</div>',
		'</div>',

		/* Draft(published) */
		'<div class="approval-status-state-boxContainer">',
			'<div class="approval-status-state-box {[this.getStatusBoxClass(values.status, "DraftPublished")]}" style="{[this.getStatusBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">',
				'<div class="approval-status-state-boxDate">{[this.getDate(values.status, values.statusHistory, "Draft(published)")]}</div>',
				'<div class="approval-status-state-boxLine draftpublished" style="{[this.getStatusColor(values.statusColors, "DraftPublished")]}">',
					'<b>Draft</b><br>(published)',
					'<div class="approval-status-state-arrow">',
						'<svg height="10" width="10">',
							'<polygon points="2.5,0 7.5,5 2.5,10" fill="#829cb8" stroke-width="0" />',
						'</svg>',	
					'</div >',
				'</div>',
				'<div toolTip="Draft created officially, displayed on the calendar, but not yet been sent to the next stage." id="discription" class="approval-status-state-boxDiscription" style="{[this.getDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">Draft created officially, displayed on the calendar, but not yet...</div>',
			'</div>',
		'</div>',

		/* OnApproval */
		'<div class="approval-status-state-boxContainer">',
			'<div class="approval-status-state-box {[this.getStatusBoxClass(values.status, "OnApproval")]}" style="{[this.getStatusBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">',
				'<div class="approval-status-state-boxDate">{[this.getDate(values.status, values.statusHistory, "On Approval")]}</div>',
				'<div class="approval-status-state-boxLine onapproval" style="{[this.getStatusColor(values.statusColors, "OnApproval")]}">',
					'<b>On Approval</b>',
					'<div class="approval-status-state-arrow">',
						'<svg height="10" width="10">',
							'<polygon points="2.5,0 7.5,5 2.5,10" fill="#829cb8" stroke-width="0" />',
						'</svg>',	
					'</div >',
				'</div>',
				'<div toolTip="Promo sent for approval and awaiting confirmation." id="discription" class="approval-status-state-boxDiscription" style="{[this.getDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">Promo sent for approval and awaiting confirmation.</div>',
			'</div>',
		'</div>',

		/* Approved */
		'<div class="approval-status-state-boxContainer">',
			'<div class="approval-status-state-box {[this.getStatusBoxClass(values.status, "Approved")]}" style="{[this.getStatusBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">',
				'<div class="approval-status-state-boxDate">{[this.getDate(values.status, values.statusHistory, "Approved")]}</div>',
				'<div class="approval-status-state-boxLine approved" style="{[this.getStatusColor(values.statusColors, "Approved")]}">',
					'<b>Approved</b>',
					'<div class="approval-status-state-arrow">',
						'<svg height="10" width="10">',
							'<polygon points="2.5,0 7.5,5 2.5,10" fill="#829cb8" stroke-width="0" />',
						'</svg>',	
					'</div >',
				'</div>',
				'<div toolTip="The promo is confirmed by all stakeholders and functions, you can send it to the client for approval. Promo is taken into account in the DMR forecast." id="discription" class="approval-status-state-boxDiscription" style="{[this.getDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">The promo is confirmed by all stakeholders and...</div>',
			'</div>',
		'</div>',

		/* Planned */
		'<div class="approval-status-state-boxContainer">',
			'<div class="approval-status-state-box {[this.getStatusBoxClass(values.status, "Planned")]}" style="{[this.getStatusBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">',
				'<div class="approval-status-state-boxDate">{[this.getDate(values.status, values.statusHistory, "Planned")]}</div>',
				'<div class="approval-status-state-boxLine planned" style="{[this.getStatusColor(values.statusColors, "Planned")]}">',
					'<b>Planned</b>',
					'<div class="approval-status-state-arrow">',
						'<svg height="10" width="10">',
							'<polygon points="2.5,0 7.5,5 2.5,10" fill="#829cb8" stroke-width="0" />',
						'</svg>',	
					'</div >',
				'</div>',
				'<div toolTip="The promo is agreed with the client, the start date of dispatch recorded." id="discription" class="approval-status-state-boxDiscription" style="{[this.getDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">The promo is agreed with the client, the start date of...</div>',
			'</div>',
		'</div>',

		/* Started */
		'<div class="approval-status-state-boxContainer">',
			'<div class="approval-status-state-box {[this.getStatusBoxClass(values.status, "Started")]}" style="{[this.getStatusBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">',
				'<div class="approval-status-state-boxDate">{[this.getDate(values.status, values.statusHistory, "Started")]}</div>',
				'<div class="approval-status-state-boxLine started" style="{[this.getStatusColor(values.statusColors, "Started")]}">',
					'<b>Started</b>',
					'<div class="approval-status-state-arrow">',
						'<svg height="10" width="10">',
							'<polygon points="2.5,0 7.5,5 2.5,10" fill="#829cb8" stroke-width="0" />',
						'</svg>',	
					'</div >',
				'</div>',
				'<div toolTip="Status comes on the day the promo starts." id="discription" class="approval-status-state-boxDiscription" style="{[this.getDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">Status comes on the day the promo starts.</div>',
			'</div>',
		'</div>',

		/* Finished */
		'<div class="approval-status-state-boxContainer">',
			'<div class="approval-status-state-box {[this.getStatusBoxClass(values.status, "Finished")]}" style="{[this.getStatusBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">',
				'<div class="approval-status-state-boxDate">{[this.getDate(values.status, values.statusHistory, "Finished")]}</div>',
				'<div class="approval-status-state-boxLine finished" style="{[this.getStatusColor(values.statusColors, "Finished")]}">',
					'<b>Finished</b>',
					'<div class="approval-status-state-arrow">',
						'<svg height="10" width="10">',
							'<polygon points="2.5,0 7.5,5 2.5,10" fill="#829cb8" stroke-width="0" />',
						'</svg>',	
					'</div >',
				'</div>',
				'<div toolTip="Status comes on the day after the end date of the promo." id="discription" class="approval-status-state-boxDiscription" style="{[this.getDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">Status comes on the day after the end date of the promo.</div>',
			'</div>',
		'</div>',

		/* Closed */
		'<div class="approval-status-state-boxContainer">',
			'<div class="approval-status-state-box {[this.getStatusBoxClass(values.status, "Closed")]}" style="{[this.getStatusBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">',
				'<div class="approval-status-state-boxDate">{[this.getDate(values.status, values.statusHistory, "Closed")]}</div>',
				'<div class="approval-status-state-boxLine closed" style="{[this.getStatusColor(values.statusColors, "Closed")]}"><b>Closed</b></div> ',
				'<div toolTip="The status comes after all the actual values have been calculated." id="discription" class="approval-status-state-boxDiscription" style="{[this.getDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">The status comes after all the actual values have been...</div>',
			'</div>',
		'</div>',
		
/*-----------------------------OnApproval FirstLine----------------------------------*/

		/* DP */
		'<div class="approval-status-state-onapproval-box" style="{[this.getOnApprovalBoxStyle("DemandPlanningNonego", values.currentWidthRatio, values.currentHeightRatio, false)]}">',
			'<div class="approval-status-state-boxDate">{[this.getOnApprovalDate(values.statusHistory, values.status, "DemandPlanningNonego")]}</div>',
			'<div class="approval-status-state-onapproval-boxLine" style="{[this.getStatusColor(values.statusColors, "OnApproval")]}"><b style="overflow:hidden;">On Approval</b></div> ',
			'<div id="onapproval-discription" class="approval-status-state-onapproval-boxDiscription" style="{[this.getOnApprovalDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}"><b>Demand Planning</b></div>',
		'</div>',
		
/*-----------------------------OnApproval SecondLine---------------------------------*/

		/* CMM */
		'<div class="approval-status-state-onapproval-box" style="{[this.getOnApprovalBoxStyle("CMManager", values.currentWidthRatio, values.currentHeightRatio, false)]}">',
			'<div class="approval-status-state-boxDate">{[this.getOnApprovalDate(values.statusHistory, values.status, "CMManager")]}</div>',
			'<div class="approval-status-state-onapproval-boxLine" style="{[this.getStatusColor(values.statusColors, "OnApproval")]}">',
				'<b style="overflow:hidden;">On Approval</b>',
				'<div class="approval-status-state-onapproval-arrow">',
					'<svg height="10" width="10">',
						'<polygon points="2.5,0 7.5,5 2.5,10" fill="#829cb8" stroke-width="0" />',
					'</svg>',
				'</div >',
			'</div> ',
			'<div id="onapproval-discription" class="approval-status-state-onapproval-boxDiscription" style="{[this.getOnApprovalDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}"><b>Customer Marketing Manager</b></div>',
		'</div>',
		
		/* DP */
		'<div class="approval-status-state-onapproval-box" style="{[this.getOnApprovalBoxStyle("DemandPlanning", values.currentWidthRatio, values.currentHeightRatio, false)]}">',
			'<div class="approval-status-state-boxDate">{[this.getOnApprovalDate(values.statusHistory, values.status, "DemandPlanning")]}</div>',
			'<div class="approval-status-state-onapproval-boxLine" style="{[this.getStatusColor(values.statusColors, "OnApproval")]}">',
				'<b style="overflow:hidden;">On Approval</b>',
			'</div > ',
			'<div id="onapproval-discription" class="approval-status-state-onapproval-boxDiscription" style="{[this.getOnApprovalDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}"><b>Demand Planning</b></div>',
		'</div>',

/*-----------------------------OnApproval ThirdLine-----------------------------------*/
		/* CMM */
		'<div class="approval-status-state-onapproval-box" style="{[this.getOnApprovalBoxStyle("CMManagerGAFirst", values.currentWidthRatio, values.currentHeightRatio, true)]}">',
			'<div class="approval-status-state-boxDate">{[this.getOnApprovalDate(values.statusHistory, values.status, "CMManagerGAFirst")]}</div>',
			'<div class="approval-status-state-onapproval-boxLine" style="{[this.getStatusColor(values.statusColors, "OnApproval")]}">',
				'<b style="overflow:hidden;">On Approval</b>',
				'<div class="approval-status-state-onapproval-arrow">',
					'<svg height="10" width="10">',
						'<polygon points="2.5,0 7.5,5 2.5,10" fill="#829cb8" stroke-width="0" />',
					'</svg>',
				'</div >',
			'</div> ',
			'<div id="onapproval-discription" class="approval-status-state-onapproval-boxDiscription" style="{[this.getOnApprovalDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}"><b>Customer Marketing Manager</b></div>',
		'</div>',

		/* DP */
		'<div class="approval-status-state-onapproval-box" style="{[this.getOnApprovalBoxStyle("DemandPlanningGA", values.currentWidthRatio, values.currentHeightRatio, true)]}">',
			'<div class="approval-status-state-boxDate">{[this.getOnApprovalDate(values.statusHistory, values.status, "DemandPlanningGA")]}</div>',
			'<div class="approval-status-state-onapproval-boxLine" style="{[this.getStatusColor(values.statusColors, "OnApproval")]}">',
				'<b style="overflow:hidden;">On Approval</b>',
				'<div class="approval-status-state-onapproval-arrow">',
					'<svg height="10" width="10">',
						'<polygon points="2.5,0 7.5,5 2.5,10" fill="#829cb8" stroke-width="0" />',
					'</svg>',
				'</div >',
			'</div > ',
			'<div id="onapproval-discription" class="approval-status-state-onapproval-boxDiscription" style="{[this.getOnApprovalDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}"><b>Demand Planning</b></div>',
		'</div>',

		/* DF */
		'<div class="approval-status-state-onapproval-box" style="{[this.getOnApprovalBoxStyle("DemandFinanceGA", values.currentWidthRatio, values.currentHeightRatio, true)]}">',
			'<div class="approval-status-state-boxDate">{[this.getOnApprovalDate(values.statusHistory, values.status, "DemandFinanceGA")]}</div>',
			'<div class="approval-status-state-onapproval-boxLine" style="{[this.getStatusColor(values.statusColors, "OnApproval")]}">',
				'<b style="overflow:hidden;">On Approval</b>',
				'<div class="approval-status-state-onapproval-arrow">',
					'<svg height="10" width="10">',
						'<polygon points="2.5,0 7.5,5 2.5,10" fill="#829cb8" stroke-width="0" />',
					'</svg>',
				'</div >',
			'</div> ',
			'<div id="onapproval-discription" class="approval-status-state-onapproval-boxDiscription" style="{[this.getOnApprovalDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}"><b>Demand Finance</b></div>',
		'</div>',

		/* CMM */
		'<div class="approval-status-state-onapproval-box" style="{[this.getOnApprovalBoxStyle("CMManagerGASecond", values.currentWidthRatio, values.currentHeightRatio, true)]}">',
			'<div class="approval-status-state-boxDate">{[this.getOnApprovalDate(values.statusHistory, values.status, "CMManagerGASecond")]}</div>',
			'<div class="approval-status-state-onapproval-boxLine" style="{[this.getStatusColor(values.statusColors, "OnApproval")]}">',
				'<b style="overflow:hidden;">On Approval</b>',
			'</div> ',
			'<div id="onapproval-discription" class="approval-status-state-onapproval-boxDiscription" style="{[this.getOnApprovalDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}"><b>Customer Marketing Manager</b></div>',
		'</div>',

		//'<br>',

		/* Deleted */
		'<div class="approval-status-state-boxContainer">',
			'<div class="approval-status-state-box {[this.getStatusBoxClass(values.status, "Deleted")]}" style="margin-left: 25px; {[this.getStatusBoxStyle(values.currentWidthRatio, values.currentHeightRatio, values.currentHeight, "Deleted")]}">',
				'<div class="approval-status-state-boxDate">{[this.getDate(values.status, values.statusHistory, "Deleted")]}</div>',
				'<div class="approval-status-state-boxLine deleted" style="{[this.getStatusColor(values.statusColors, "Deleted")]}"><b>Deleted</b></div> ',
				'<div toolTip="Promo is completely deleted." id="discription" class="approval-status-state-boxDiscription" style="{[this.getDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">Promo is completely deleted.</div>',
			'</div>',
		'</div>',

		/* Canceled */
		'<div class="approval-status-state-boxContainer" style="{[this.getCancelledBoxStyle(values.currentHeightRatio)]}">',
			'<div class="approval-status-state-box {[this.getStatusBoxClass(values.status, "Canceled")]}" style="{[this.getStatusBoxStyle(values.currentWidthRatio, values.currentHeightRatio, values.currentHeight, "Canceled")]}">',
				'<div class="approval-status-state-boxDate">{[this.getDate(values.status, values.statusHistory, "Cancelled")]}</div>',
				'<div class="approval-status-state-boxLine canceled" style="{[this.getStatusColor(values.statusColors, "Cancelled")]}"><b>Cancelled</b></div> ',
				'<div toolTip="Promo cancelled at the initiative of the client or KAM." id="discription" class="approval-status-state-boxDiscription" style="{[this.getDiscriptionBoxStyle(values.currentWidthRatio, values.currentHeightRatio)]}">Promo cancelled at the initiative of the client or KAM.</div>',
			'</div>',
		'</div>',
		
        {
			getStatusSelectorStyle: function (promoStatus, currentWidthRatio, currentHeightRatio, currentHeight) {
				var left = 25;
				var top = 25;
				var boxWidth = 130 * currentWidthRatio;
				var boxHeight = 130 * currentHeightRatio;

				switch (promoStatus) {
					case 'DraftPublished':
						left += boxWidth + 10;
						break;
					case 'OnApproval':
						left += 2 * boxWidth + 20;
						break;
					case 'Approved':
						left += 3 * boxWidth + 30;
						break;
					case 'Planned':
						left += 4 * boxWidth + 40;
						break;
					case 'Started':
						left += 5 * boxWidth + 50;
						break;
					case 'Finished':
						left += 6 * boxWidth + 60;
						break;
					case 'Closed':
						left += 7 * boxWidth + 70;
						break;
					case 'Deleted':
						left += boxWidth + 10;
						break;
					case 'Cancelled':
						top += 130 * currentHeightRatio + (130 * currentHeightRatio - 7) - 2;
						left += 5 * boxWidth + 50;
						break;
					default:
						break;
				}
				return 'top:' + (top + 2) + 'px; left:' + (left + 2) + 'px; width:' + (boxWidth - 4) + 'px; height:' + (boxHeight - 4) + 'px;';
			},

			getStateSelectorStyle: function (promoStatus, isNonego, onApprovalState, currentWidthRatio, currentHeightRatio) {
				if (promoStatus == 'OnApproval') {
					var boxWidth = 130 * currentWidthRatio;
					var boxHeight = 130 * currentHeightRatio * 0.85;
					var left = boxWidth * 2.5 + 25;
					var top = 130 * currentHeightRatio + 40;
					switch (onApprovalState) {
						case 'CMManager':
							top += 130 * currentHeightRatio - 7;
							break;
						case 'DemandPlanning':
							top += 130 * currentHeightRatio - 7;
							left += boxWidth + 10;
							break;
						case 'DemandFinance':
							left += 2 * boxWidth;
							break;
						case 'ErrorApproval':
							return 'display: none;';
						case 'DemandPlanningNonego':
							break;

						case 'CMManagerGAFirst':
							top += 260 * currentHeightRatio - 14;
							break;
						case 'CMManagerGASecond':
							top += 260 * currentHeightRatio - 14;
							left += 3 * boxWidth + 10 * 3;
							break;
						case 'DemandPlanningGA':
							top += 260 * currentHeightRatio - 14;
							left += boxWidth + 10;
							break;
						case 'DemandFinanceGA':
							top += 260 * currentHeightRatio - 14;
							left += 2 * boxWidth + 10 * 2;
							break;
						default:
							break;
					}

					return 'top:' + (top + 2) + 'px; left:' + (left + 2) + 'px;' + 'width:' + (boxWidth - 4) + 'px; height:' + (boxHeight - 4) + 'px;';
				} else {
					return 'display: none;';
				}
			},

			getCancelledBoxStyle: function (currentHeightRatio) {
				var top = 130 * currentHeightRatio - 35;
				return 'position: absolute; margin-top: ' + top + 'px'; 
			},

			getOnApprovalBoxStyle: function (boxRole, currentWidthRatio, currentHeightRatio) {
				var boxOffset = 130 * currentWidthRatio + 10;
				var boxWidth = 130 * currentWidthRatio;
				var boxHeight = 130 * currentHeightRatio * 0.85;
				var left = boxOffset * 2.5;
				var top = 130 * currentHeightRatio + 40;
				switch (boxRole) {
					case 'CMManager':
						top += 130 * currentHeightRatio - 7;
						break;
					case 'DemandPlanning':
						top += 130 * currentHeightRatio - 7;
						left += boxOffset;
						//left += boxWidth - 130;
						break;
					case 'DemandFinance':
						left += 2 * boxOffset;
						break;

					case 'DemandPlanningNonego':
						break;

					case 'CMManagerGAFirst':
						top += 260 * currentHeightRatio - 14;
						break;
					case 'CMManagerGASecond':
						top += 260 * currentHeightRatio - 14;
						left += 3 * boxOffset;
						break;
					case 'DemandPlanningGA':
						top += 260 * currentHeightRatio - 14;
						left += boxOffset;
						//left += boxWidth - 130;
						break;
					case 'DemandFinanceGA':
						top += 260 * currentHeightRatio - 14;
						left += 2 * boxOffset;
						break;
				}
				return 'left:' + left + 'px; top:' + top + 'px; width:' + boxWidth + 'px; height:' + boxHeight + 'px;';
			},

			getDiscriptionBoxStyle: function (currentWidthRatio, currentHeightRatio) {
				var boxHeight = 130 * currentHeightRatio;
				var boxWidth = 130 * currentWidthRatio;
				var discriptionBoxHeight = boxHeight - 57;
				return 'height:' + discriptionBoxHeight + 'px; width:' + boxWidth + 'px;';
			},

			getOnApprovalDiscriptionBoxStyle: function (currentWidthRatio, currentHeightRatio) {
				var boxHeight = 130 * currentHeightRatio - 15;
				var boxWidth = 130 * currentWidthRatio;
				var discriptionBoxHeight = boxHeight - 37;
				var fontsize = 'smaller';

				if (boxWidth < 110) {
					fontsize = 'x-small';
				} else if (boxWidth < 105) {
					fontsize = 'xx-small';
				}
				
				return 'height:' + discriptionBoxHeight + 'px; width:' + boxWidth + 'px; font-size:' + fontsize;
			},

			getStatusBoxStyle: function (currentWidthRatio, currentHeightRatio, currentHeight, name) {
				var boxWidth = 130 * currentWidthRatio;
				var boxHeight = 130 * currentHeightRatio;
				var marginTop = currentHeight - 2 * boxHeight - 52;

				if (name == "Canceled") {
					var boxOffset = boxWidth * 4 + 40;
					return 'width:' + boxWidth + 'px; height:' + boxHeight + 'px; margin-left:' + boxOffset + 'px;';
				} else if (name == "Deleted") {
					var topOffset = currentHeight - boxHeight - marginTop - 25;
					return 'width:' + boxWidth + 'px; height:' + boxHeight + 'px;' + 'margin-top:' + marginTop + 'px; margin-bottom:' + 25 + 'px;';
				}

				return 'width:' + boxWidth + 'px; height:' + boxHeight + 'px;';
			},

			getStatusBoxClass: function (promoStatus, boxStatus) {
				return promoStatus == boxStatus ? 'selected' : '';
			},

			getStatusColor: function (statusColors, statusName) {
				return 'background-color:' + statusColors[statusName]
			},

			getDate: function (currentStatus, statusHistory, statusName) {
				var statuses = ['Draft', 'DraftPublished', 'OnApproval', 'Approved', 'Planned', 'Started', 'Finished', 'Closed', 'Deleted', 'Cancelled'];
				var reversed = false;
				var tempStatus = statusName;
				statuses = statuses.slice(0, statuses.indexOf(currentStatus) + 1);

				if (statusName == 'Draft(published)') {
					tempStatus = 'DraftPublished';
				} else if (statusName == 'On Approval') {
					tempStatus = 'OnApproval';
				}
				if (statuses.indexOf(tempStatus) == -1) {
					return '';
				}

				if (statusName == 'On Approval') {
					statusHistory = statusHistory.reverse();
					reversed = !reversed;
				}
				
				for (var i = 0; i < statusHistory.length; i++) {
					if (statusHistory[i].StatusName == statusName) {
						var dateToReturn = Ext.util.Format.date(statusHistory[i].Date, 'd.m.Y H:i');
						if (reversed) {
							statusHistory = statusHistory.reverse();
							reversed = !reversed;
						}
						return dateToReturn;
					}
				}
				if (reversed) {
					statusHistory = statusHistory.reverse();
					reversed = !reversed;
				}
				return '';
			},

			getOnApprovalDate: function (statusHistory, currentStatus, boxRole) {
				var statusesBeforeOnApproval = ['Draft', 'DraftPublished'];
				var firstOnApprovalStatus = null;
				var firstOnApprovalRoleName = '';
				var firstApprovedStatus = null;
				var firstApprovedRoleName = '';
				var date = '';

				if (statusHistory.length == 0) {
					return;
				}
				var promo = statusHistory[0].Promo;

				// Скрываем даты ещё не аппрувнутых статусов, даже если есть записи о их поддтверждении в прошлом
				if (!promo.IsCMManagerApproved && !promo.IsDemandPlanningApproved && !promo.IsDemandFinanceApproved) {
					return '';
				} else if (statusesBeforeOnApproval.indexOf(currentStatus) != -1) {
					return '';
				} else if (!promo.IsDemandPlanningApproved && !promo.IsDemandFinanceApproved && (boxRole == 'DemandFinance' || boxRole == 'DemandPlanning')) {
					return '';
				} else if (!promo.IsDemandFinanceApproved && boxRole == 'DemandFinance') {
					return '';
				}

				for (var i = 0; i < statusHistory.length; i++) {
					if (statusHistory[i].StatusName == "On Approval") {
						firstOnApprovalStatus = statusHistory[i];
						firstOnApprovalRoleName = firstOnApprovalStatus.RoleName;
						break;
					}
				}
				for (var i = 0; i < statusHistory.length; i++) {
					if (statusHistory[i].StatusName == "Approved") {
						firstApprovedStatus = statusHistory[i];
						firstApprovedRoleName = firstApprovedStatus.RoleName;
						break;
					}
				}

				if (promo.IsGrowthAcceleration === false && promo.IsInExchange === false) {
					//CMM
					if (boxRole === 'CMManager'
						&& promo.IsCMManagerApproved
						&& !promo.IsDemandFinanceApproved
						&& statusesBeforeOnApproval.indexOf(currentStatus) == -1) {
						for (var i = 0; i < statusHistory.length; i++) {
							if (statusHistory[i].StatusName == "On Approval" && statusHistory[i].RoleName == 'Customer Marketing Manager') {
								date = Ext.util.Format.date(statusHistory[i].Date, 'd.m.Y H:i');
								break;
							}
						}
						return date;
					}
					//DP не nonego
					if (boxRole === 'DemandPlanning'
						&& promo.IsDemandPlanningApproved 
						&& !promo.IsDemandFinanceApproved
						&& statusesBeforeOnApproval.indexOf(currentStatus) == -1
						&& currentStatus != 'OnApproval') {
						for (var i = 0; i < statusHistory.length; i++) {
							if (statusHistory[i].StatusName == "Approved" && statusHistory[i].RoleName == 'Demand Planning') {
								date = Ext.util.Format.date(statusHistory[i].Date, 'd.m.Y H:i');
								break;
							}
						}
						return date;
					}
					//DP Nonego
					if (boxRole === 'DemandPlanningNonego'
						&& promo.IsDemandPlanningApproved
						&& promo.IsDemandFinanceApproved
						&& statusesBeforeOnApproval.indexOf(currentStatus) == -1
						&& currentStatus != 'OnApproval') {
						for (var i = 0; i < statusHistory.length; i++) {
							if (statusHistory[i].StatusName == "Approved" && statusHistory[i].RoleName == 'Demand Planning') {
								date = Ext.util.Format.date(statusHistory[i].Date, 'd.m.Y H:i');
								break;
							}
						}
						return date;
					}
				}
				else if (promo.IsGrowthAcceleration === true || promo.IsInExchange === true) {
					//CMM First
					if (boxRole === 'CMManagerGAFirst'
						&& statusesBeforeOnApproval.indexOf(currentStatus) == -1
						&& (promo.IsCMManagerApproved
							|| (promo.IsDemandPlanningApproved && promo.IsDemandFinanceApproved && !promo.IsCMManagerApproved))) {
						for (var i = statusHistory.length - 1; i >= 0; i--) {
							if (statusHistory[i].StatusName == "On Approval" && statusHistory[i].RoleName == 'Customer Marketing Manager') {
								date = Ext.util.Format.date(statusHistory[i].Date, 'd.m.Y H:i');
								break;
							}
						}
						return date;
					}
					//CMM Second
					if (boxRole === 'CMManagerGASecond'
						&& promo.IsCMManagerApproved
						&& statusesBeforeOnApproval.indexOf(currentStatus) == -1
						&& (promo.IsDemandPlanningApproved && promo.IsDemandFinanceApproved)) {
						for (var i = 0; i < statusHistory.length; i++) {
							if (statusHistory[i].StatusName == "Approved" && statusHistory[i].RoleName == 'Customer Marketing Manager') {
								date = Ext.util.Format.date(statusHistory[i].Date, 'd.m.Y H:i');
								break;
							}
						}
						return date;
					}
					//DP
					if (boxRole === 'DemandPlanningGA'
						&& promo.IsDemandPlanningApproved
						&& statusesBeforeOnApproval.indexOf(currentStatus) == -1) {
						for (var i = 0; i < statusHistory.length; i++) {
							if (statusHistory[i].StatusName == "On Approval" && statusHistory[i].RoleName == 'Demand Planning') {
								date = Ext.util.Format.date(statusHistory[i].Date, 'd.m.Y H:i');
								break;
							}
						}
						return date;
					}
					//DF
					if (boxRole === 'DemandFinanceGA'
						&& promo.IsDemandFinanceApproved
						&& statusesBeforeOnApproval.indexOf(currentStatus) == -1) {
						for (var i = 0; i < statusHistory.length; i++) {
							if (statusHistory[i].StatusName == "On Approval" && statusHistory[i].RoleName == 'Demand Finance') {
								date = Ext.util.Format.date(statusHistory[i].Date, 'd.m.Y H:i');
								break;
							}
						}
						return date;
					}
				}
			},

			getLinePoints: function (name, currentHeightRatio, currentWidthRatio, currentHeight) {
				var boxWidth = 140 * currentWidthRatio;
				var boxHeight = 130 * currentHeightRatio;
				var topOffset = currentHeight - boxHeight - 25;
				switch (name) {
					case 'FromOnApproval':
						return (boxWidth * 2.5) + ',' + 25
							+ ' ' + (boxWidth * 2.5) + ',' + 12.5
							+ ' ' + (boxWidth * 1.5 + 25) + ',' + 12.5
							+ ' ' + (boxWidth * 1.5 + 25) + ',' + 20;
					case 'FromApproved':
						return (boxWidth * 3.5 + 25) + ',' + 25
							+ ' ' + (boxWidth * 3.5 + 25) + ',' + 12.5
							+ ' ' + (boxWidth * 2.7 + 25) + ',' + 12.5
							+ ' ' + (boxWidth * 2.7 + 25) + ',' + 20;
					case 'FromPlanned':
						return (boxWidth * 4.5 + 25) + ',' + 25
							+ ' ' + (boxWidth * 4.5 + 25) + ',' + 2.5
							+ ' ' + (boxWidth * 2.5 + 25) + ',' + 2.5
							+ ' ' + (boxWidth * 2.5 + 25) + ',' + 20;
					case 'ToDeleted':
						var x = (boxWidth * 1.5 + 25) + ',' + (25 + boxHeight)
							+ ' ' + (boxWidth * 1.5 + 25) + ',' + (25 + boxHeight + 20)
							+ ' ' + (boxWidth * 0.5 + 25) + ',' + (25 + boxHeight + 20)
							+ ' ' + (boxWidth * 0.5 + 25) + ',' + (25 + boxHeight)
							+ ' ' + (boxWidth * 0.5 + 25) + ',' + (topOffset - 5);
						return x;
					case 'ToCanceled':
						return (boxWidth * 3.5 + 25) + ',' + (25 + boxHeight)
							+ ' ' + (boxWidth * 3.5 + 25) + ',' + (25 + boxHeight + 20)
							+ ' ' + (boxWidth * 4.5 + 25) + ',' + (25 + boxHeight + 20)
							+ ' ' + (boxWidth * 4.5 + 25) + ',' + (25 + boxHeight)
							+ ' ' + (boxWidth * 4.5 + 25) + ',' + (25 + boxHeight + 20)
							+ ' ' + (boxWidth * 5.5 + 25) + ',' + (25 + boxHeight + 20)
							+ ' ' + (boxWidth * 5.5 + 25) + ',' + (25 + boxHeight)
							+ ' ' + (boxWidth * 5.5 + 25) + ',' + (20 + 2 * boxHeight - 10);
					case 'ToApproval':
						return      (boxWidth * 2.2 + 25) + ',' + (25 + boxHeight)
							+ ' ' + (boxWidth * 2.2 + 25) + ',' + (25 + boxHeight + 39)
							+ ' ' + (boxWidth * 2.5 - 5) + ',' + (25 + boxHeight + 39)

							+ ' ' + (boxWidth * 2.2 + 25) + ',' + (25 + boxHeight + 39)
							+ ' ' + (boxWidth * 2.2 + 25) + ',' + (25 + 2 * boxHeight + 32)
							+ ' ' + (boxWidth * 2.5 - 5) + ',' + (25 + 2 * boxHeight + 32)

							+ ' ' + (boxWidth * 2.2 + 25) + ',' + (25 + 2 * boxHeight + 32)
							+ ' ' + (boxWidth * 2.2 + 25) + ',' + (25 + 3 * boxHeight + 25)
							+ ' ' + (boxWidth * 2.5 - 5) + ',' + (25 + 3 * boxHeight + 25);
				}
			},

			getPointerPoints: function (name, currentHeightRatio, currentWidthRatio, currentHeight) {
				var boxWidth = 140 * currentWidthRatio;
				var boxOffset = (130 * currentWidthRatio + 10) * 2.5;
				var boxHeight = 130 * currentHeightRatio;
				var bottomOffset = currentHeight - boxHeight - 25;
				switch (name) {
					case 'FromOnApproval':
						return (boxWidth * 1.5 + 20) + ',' + 18
							+ ' ' + (boxWidth * 1.5 + 25) + ',' + 23
							+ ' ' + (boxWidth * 1.5 + 30) + ',' + 18;
						break;
					case 'FromApproved':
						return (boxWidth * 2.7 + 20) + ',' + 18
							+ ' ' + (boxWidth * 2.7 + 25) + ',' + 23
							+ ' ' + (boxWidth * 2.7 + 30) + ',' + 18;
						break;
					case 'FromPlanned':
						return (boxWidth * 2.5 + 20) + ',' + 18
							+ ' ' + (boxWidth * 2.5 + 25) + ',' + 23
							+ ' ' + (boxWidth * 2.5 + 30) + ',' + 18;
						break;
					case 'ToDeleted':
						return (boxWidth * 0.5 + 20) + ',' + (bottomOffset - 9)
							+ ' ' + (boxWidth * 0.5 + 25) + ',' + (bottomOffset - 4)
							+ ' ' + (boxWidth * 0.5 + 30) + ',' + (bottomOffset - 9);
						break;
					case 'ToCanceled':
						return (boxWidth * 5.5 + 20) + ',' + (25 + 2 * boxHeight - 17)
							+ ' ' + (boxWidth * 5.5 + 25) + ',' + (25 + 2 * boxHeight - 12)
							+ ' ' + (boxWidth * 5.5 + 30) + ',' + (25 + 2 * boxHeight - 17);
						break;
					case 'ToApprovalFirst':
						return      (boxOffset - 7) + ',' + (25 + boxHeight + 34)
							+ ' ' + (boxOffset - 2)     + ',' + (25 + boxHeight + 39)
							+ ' ' + (boxOffset - 7) + ',' + (25 + boxHeight + 44);
						break;
					case 'ToApprovalSecond':
						return      (boxOffset - 7) + ',' + (25 + 2 * boxHeight + 27)
							+ ' ' + (boxOffset - 2)     + ',' + (25 + 2 * boxHeight + 32)
							+ ' ' + (boxOffset - 7) + ',' + (25 + 2 * boxHeight + 37);
						break;
					case 'ToApprovalThird':
						return      (boxOffset - 7) + ',' + (25 + 3 * boxHeight + 20)
							+ ' ' + (boxOffset - 2)     + ',' + (25 + 3 * boxHeight + 25)
							+ ' ' + (boxOffset - 7) + ',' + (25 + 3 * boxHeight + 30);
						break;
				}
			},
        }
    ])
})