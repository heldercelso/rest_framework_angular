from django.contrib import admin

# Register your models here.
from fee_app.models import FeesCharged, Client, SegmentFee

# Register your models here.
admin.site.register(Client)
admin.site.register(FeesCharged)
admin.site.register(SegmentFee)