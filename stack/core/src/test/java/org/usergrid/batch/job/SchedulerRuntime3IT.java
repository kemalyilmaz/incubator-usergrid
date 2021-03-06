/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package org.usergrid.batch.job;


import org.usergrid.cassandra.Concurrent;
import org.usergrid.persistence.entities.JobData;
import org.usergrid.persistence.entities.JobStat;

import org.junit.Test;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertTrue;


/**
 * Class to test job runtimes
 */
@Concurrent
public class SchedulerRuntime3IT extends AbstractSchedulerRuntimeIT {


    /** Test the scheduler ramps up correctly when there are more jobs to be read after a pause */
    @Test
    public void failureCausesJobDeath() throws Exception {

        int failCount = Integer.parseInt( props.getProperty( FAIL_PROP ) );
        long sleepTime = Long.parseLong( props.getProperty( RUNLOOP_PROP ) );

        FailureJobExceuction job = cassandraResource.getBean( "failureJobExceuction", FailureJobExceuction.class );

        int latchValue = failCount + 1;

        job.setLatch( latchValue );

        JobData returned = scheduler.createJob( "failureJobExceuction", System.currentTimeMillis(), new JobData() );

        // sleep until the job should have failed. We sleep 1 extra cycle just to
        // make sure we're not racing the test
        boolean waited = getJobListener().blockTilDone( 3, ( failCount + 2 ) * sleepTime + 5000L );

        //we shouldn't trip the latch.  It should fail failCount times, and not run again
        assertTrue( "Jobs ran", waited );
        assertTrue( failCount + " failures resulted", getJobListener().getFailureCount() == failCount );
        assertTrue( 1 + " success resulted", getJobListener().getSuccessCount() == 1 );

        //we shouldn't have run the last time, we should have counted down to it
        assertEquals( 1, job.getLatchCount() );

        JobStat stat = scheduler.getStatsForJob( returned.getJobName(), returned.getUuid() );

        // we should have only marked this as run fail+1 times
        assertEquals( latchValue, stat.getTotalAttempts() );
        assertEquals( latchValue, stat.getRunCount() );
        assertEquals( 0, stat.getDelayCount() );
    }
}
