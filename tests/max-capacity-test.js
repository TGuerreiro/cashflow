import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  scenarios: {
    stress_test: {
      executor: 'ramping-arrival-rate',
      startRate: 50,              // Começa com o requisito do PDF (50 req/s )
      timeUnit: '1s',
      preAllocatedVUs: 100,       // Reserva recursos para o teste
      maxVUs: 1000,               // Permite escalar se necessário
      stages: [
        { target: 100, duration: '30s' },  // Sobe para 100 req/s
        { target: 250, duration: '30s' },  // Sobe para 250 req/s
        { target: 500, duration: '1m' },   // Tenta chegar a 500 req/s (10x o requisito)
        { target: 0, duration: '30s' },    // Resfriamento
      ],
    },
  },
  thresholds: {
    // Mantemos apenas a falha para o k6 não dar erro de sintaxe no Windows
    'http_req_failed': ['rate<0.05'], 
  },
};

export default function ( ) {
  const url = 'http://localhost:5001/api/lancamentos';
  const payload = JSON.stringify({
    data: new Date( ).toISOString().split('T')[0],
    valor: (Math.random() * 1000).toFixed(2),
    tipo: Math.random() > 0.5 ? 1 : 2,
    descricao: 'Busca de Limite Máximo RPS'
  });

  const params = {
    headers: { 'Content-Type': 'application/json' },
  };

  const res = http.post(url, payload, params );

  check(res, {
    'status is 200 or 201': (r) => r.status === 200 || r.status === 201,
  });
}
