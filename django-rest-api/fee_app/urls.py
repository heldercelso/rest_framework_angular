from django.conf.urls import url
from fee_app import views

urlpatterns = [
    url(r'^api/fees-charged$', views.fee_list),
    url(r'^api/fees-charged/(?P<cpf>[0-9]+)/(?P<id>[0-9]+)$', views.fee_detail),
    url(r'^api/new-client$', views.new_client),
    url(r'^api/new-segment$', views.new_segment),
]
