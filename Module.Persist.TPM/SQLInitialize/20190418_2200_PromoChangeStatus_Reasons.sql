Update PromoStatusChange 
	Set RejectReasonId = ISNULL((Select Id From RejectReason Where [Name] = Comment), (Select Id From RejectReason Where [SystemName]='Other')),
		Comment = NULL
	Where Comment IS NOT NULL AND (Select Id From RejectReason Where [Name] = Comment) IS NOT NULL;

Update PromoStatusChange 
	Set RejectReasonId = ISNULL((Select Id From RejectReason Where [Name] = Comment), (Select Id From RejectReason Where [SystemName]='Other'))
	Where Comment IS NOT NULL AND (Select Id From RejectReason Where [Name] = Comment) IS NULL;