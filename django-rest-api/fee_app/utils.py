from urllib.parse import urlencode, urlparse, parse_qs
from fee_project.settings import API_KEY
import requests

def merge_url_query_params(url: str, additional_params: dict) -> str:
    url_components = urlparse(url)
    original_params = parse_qs(url_components.query)
    # Before Python 3.5 you could update original_params with 
    # additional_params, but here all the variables are immutable.
    merged_params = {**original_params, **additional_params}
    updated_query = urlencode(merged_params, doseq=True)
    # _replace() is how you can create a new NamedTuple with a changed field
    return url_components._replace(query=updated_query).geturl()


def get_current_fee():
    base_url = 'http://api.exchangeratesapi.io/v1/latest'
    params = {'access_key': API_KEY,
            #   'base': 'USD', # not supported on free plan (default EUR)
                'symbols': 'BRL',
                'format': '1'}
    url = merge_url_query_params(base_url, params)
    response = requests.get(url)

    if response.status_code == requests.codes.ok:
        json_content = response.json()
        fee = float(json_content['rates']['BRL'])
    else:
        fee = 4.7776 # default fee
    # fee = 4.7776 # default fee

    return fee