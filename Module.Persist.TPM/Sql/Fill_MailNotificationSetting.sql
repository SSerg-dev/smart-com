DELETE FROM [MailNotificationSetting]
GO

INSERT INTO [MailNotificationSetting] 
([Id], [Name],[Description],[Subject],[Body],[IsDisabled],[To],[CC],[BCC],[Disabled],[DeletedDate])
VALUES 
(NEWID(), 'CANCELLED_PROMO_NOTIFICATION', 'Notification of the cancelling promo', 'Promoes have been cancelled',
'#HTML_SCAFFOLD#', 0, NULL, NULL, 'monitoring@smartcom.support', 0, NULL),
(NEWID(), 'PROMO_DEMAND_CHANGE_NOTIFICATION', 'Notification about changes in promo', 'Promos have been changed',
'#HTML_SCAFFOLD#', 0, NULL, NULL, 'monitoring@smartcom.support', 0, NULL),
(NEWID(), 'PROMO_PRODUCT_CREATE_NOTIFICATION', 'New products relevant for the following promotions were found', 'New products relevant for the following promotions were found',
'#HTML_SCAFFOLD#', 0, NULL, NULL, 'monitoring@smartcom.support', 0, NULL),
(NEWID(), 'PROMO_PRODUCT_DELETE_NOTIFICATION', 'Products used in the following promotions no longer match the conditions', 'Products used in the following promotions no longer match the conditions',
'#HTML_SCAFFOLD#', 0, NULL, NULL, 'monitoring@smartcom.support', 0, NULL),
(NEWID(), 'PROMO_UPLIFT_FAIL_NOTIFICATION', 'Failed to calculate uplift for the following promoes', 'Failed to calculate uplift for the following promoes',
'#HTML_SCAFFOLD#', 0, NULL, NULL, 'monitoring@smartcom.support', 0, NULL),
(NEWID(), 'WEEK_BEFORE_DISPATCH_PROMO_NOTIFICATION', 'Notification of promoes that have a week before dispatch start', 'Promoes that have a week left before dispatch start date',
'#HTML_SCAFFOLD#', 0, NULL, NULL, 'monitoring@smartcom.support', 0, NULL),
(NEWID(), 'PROMO_ON_APPROVAL_NOTIFICATION', 'Notification of promoes that need approval', 'Promoes that need approval',
'#HTML_SCAFFOLD#', 0, NULL, NULL, 'monitoring@smartcom.support', 0, NULL),
(NEWID(), 'PROMO_ON_REJECT_NOTIFICATION', 'Notification of promoes that have been rejected', 'Promoes that have been rejected',
'#HTML_SCAFFOLD#', 0, NULL, NULL, 'monitoring@smartcom.support', 0, NULL),
(NEWID(), 'PROMO_APPROVED_NOTIFICATION', 'Notification of promoes that have been approved', 'Promoes that have been approved',
'#HTML_SCAFFOLD#', 0, NULL, NULL, 'monitoring@smartcom.support', 0, NULL)