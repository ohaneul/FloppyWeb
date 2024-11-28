from dataclasses import dataclass
from typing import Optional, Dict
import re
import tldextract
from urllib.parse import urlparse, parse_qs

@dataclass
class ParsedURL:
    scheme: str
    domain: str
    tld: str
    path: str
    query_params: Dict[str, list]
    fragment: Optional[str]
    port: Optional[int]
    username: Optional[str]
    password: Optional[str]

class URLParser:
    def __init__(self):
        self.url_regex = re.compile(
            r'^(?:(?P<scheme>[a-z]+)://)?'
            r'(?:(?P<auth>[^@]+)@)?'
            r'(?P<host>[^/:]+)'
            r'(?::(?P<port>\d+))?'
            r'(?P<path>/[^?#]*)?'
            r'(?:\?(?P<query>[^#]*))?'
            r'(?:#(?P<fragment>.*))?$'
        )
        
    def parse(self, url: str) -> ParsedURL:
        parsed = urlparse(url)
        extracted = tldextract.extract(url)
        query_params = parse_qs(parsed.query)
        
        return ParsedURL(
            scheme=parsed.scheme or 'https',
            domain=extracted.domain,
            tld=extracted.suffix,
            path=parsed.path,
            query_params=query_params,
            fragment=parsed.fragment,
            port=parsed.port,
            username=parsed.username,
            password=parsed.password
        )

    def validate(self, url: str) -> bool:
        try:
            parsed = self.parse(url)
            return all([
                parsed.scheme in ('http', 'https', 'ftp', 'file'),
                len(parsed.domain) > 0,
                len(parsed.tld) > 0
            ])
        except Exception:
            return False 